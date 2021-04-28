using Albion.Code;
using Albion.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Albion.Engine
{
    public partial class FilteringEngine
    {
        private IEnumerable<FWPM_FILTER0> GetFilters()
        {
            var enumHandle = IntPtr.Zero;
            var entries = IntPtr.Zero;

            try
            {
                var code = Methods.FwpmFilterCreateEnumHandle0(engineHandle, IntPtr.Zero, out enumHandle);
                if (code != 0)
                    throw new NativeException(nameof(Methods.FwpmFilterCreateEnumHandle0), code);

                code = Methods.FwpmFilterEnum0(engineHandle, enumHandle, uint.MaxValue, out entries, out uint numEntriesReturned);
                if (code != 0)
                    throw new NativeException(nameof(Methods.FwpmFilterEnum0), code);

                var filterSize = Marshal.SizeOf<FWPM_FILTER0>();
                for (uint i = 0; i < numEntriesReturned; i++)
                {
                    var ptr = new IntPtr(entries.ToInt64() + i * IntPtr.Size);
                    var ptr2 = Marshal.PtrToStructure<IntPtr>(ptr);
                    var filter = Marshal.PtrToStructure<FWPM_FILTER0>(ptr2);

                    if (filter.providerKey == IntPtr.Zero)
                        continue;

                    var filterProviderKey = Marshal.PtrToStructure<Guid>(filter.providerKey);
                    if (filterProviderKey != providerKey)
                        continue;

                    yield return filter;
                }
            }
            finally
            {
                if (entries != IntPtr.Zero)
                    Methods.FwpmFreeMemory0(ref entries);

                if (enumHandle != IntPtr.Zero)
                {
                    var code = Methods.FwpmFilterDestroyEnumHandle0(engineHandle, enumHandle);
                    if (code != 0)
                        throw new NativeException(nameof(Methods.FwpmFilterDestroyEnumHandle0), code);
                }
            }
        }

        public void DeleteFilter(Guid key)
        {
            var code = Methods.FwpmFilterDeleteByKey0(engineHandle, ref key);
            if (code != 0 && code != (uint)FWP_E.FILTER_NOT_FOUND)
                throw new NativeException(nameof(Methods.FwpmFilterDeleteByKey0), code);
        }

        public void ClearFilters()
        {
            foreach (var filter in GetFilters())
                DeleteFilter(filter.filterKey);
        }

        public void SetSilentBlockInV4() => SetSilentBlockIn(false);
        public void SetSilentBlockInV6() => SetSilentBlockIn(true);

        public void SetSilentBlockIn(bool v6)
        {
            var layerKey = v6 ? Layers.FWPM_LAYER_INBOUND_TRANSPORT_V6_DISCARD : Layers.FWPM_LAYER_INBOUND_TRANSPORT_V4_DISCARD;
            var calloutKey = v6 ? Callouts.FWPM_CALLOUT_WFP_TRANSPORT_LAYER_V6_SILENT_DROP : Callouts.FWPM_CALLOUT_WFP_TRANSPORT_LAYER_V4_SILENT_DROP;
            AddFilter(FWP_ACTION_TYPE.CALLOUT_TERMINATING, calloutKey, layerKey, null);
        }

        public void AddFilterInV4(bool permit, IPPROTO[] protocols = null, string[] localRules = null, string[] remoteRules = null) =>
            AddFilter(permit, false, false, protocols, localRules, remoteRules);

        public void AddFilterInV6(bool permit, IPPROTO[] protocols = null, string[] localRules = null, string[] remoteRules = null) =>
            AddFilter(permit, false, true, protocols, localRules, remoteRules);

        public void AddFilterOutV4(bool permit, IPPROTO[] protocols = null, string[] localRules = null, string[] remoteRules = null) =>
            AddFilter(permit, true, false, protocols, localRules, remoteRules);

        public void AddFilterOutV6(bool permit, IPPROTO[] protocols = null, string[] localRules = null, string[] remoteRules = null) =>
            AddFilter(permit, true, true, protocols, localRules, remoteRules);

        public void AddFilter(bool permit, bool output, bool v6, IPPROTO[] protocols = null, string[] localRules = null, string[] remoteRules = null)
        {
            using var ptrs = new NativePtrs();

            var actionType = permit ? FWP_ACTION_TYPE.PERMIT : FWP_ACTION_TYPE.BLOCK;
            var layerKey = output ?
                (v6 ? Layers.FWPM_LAYER_ALE_AUTH_CONNECT_V6 : Layers.FWPM_LAYER_ALE_AUTH_CONNECT_V4) :
                (v6 ? Layers.FWPM_LAYER_ALE_AUTH_RECV_ACCEPT_V6 : Layers.FWPM_LAYER_ALE_AUTH_RECV_ACCEPT_V4);

            var conditions = Enumerable.Empty<FWPM_FILTER_CONDITION0>()
                .ConcatSafe(CreateConditionsProtocol(protocols))
                .ConcatSafe(CreateConditionsPortOrSubnet(v6, false, localRules, ptrs))
                .ConcatSafe(CreateConditionsPortOrSubnet(v6, true, remoteRules, ptrs))
                .ToArray();

            AddFilter(actionType, Guid.Empty, layerKey, conditions);
        }

        public FWPM_FILTER0 AddFilter(FWP_ACTION_TYPE actionType, Guid calloutKey, Guid layerKey, FWPM_FILTER_CONDITION0[] conditions)
        {
            using var ptrs = new NativePtrs();

            var filter = new FWPM_FILTER0();
            filter.providerKey = ptrs.Add(providerKey);
            filter.filterKey = Guid.NewGuid();
            filter.layerKey = layerKey;
            filter.subLayerKey = SubLayers.Get(layerKey);
            filter.flags = FWPM_FILTER_FLAG.PERSISTENT;
            filter.action.type = actionType;
            filter.action.calloutKey = calloutKey;
            filter.weight.type = FWP_DATA_TYPE.UINT8;
            filter.weight.value.uint8 = (actionType == FWP_ACTION_TYPE.PERMIT) ? (byte)1 : (byte)0;
            filter.displayData.name = filter.filterKey.ToString();

            if (conditions != null && conditions.Length > 0)
            {
                int conditionSize = Marshal.SizeOf<FWPM_FILTER_CONDITION0>();
                var filterConditions = ptrs.Add(conditionSize * conditions.Length);

                for (int i = 0; i < conditions.Length; i++)
                {
                    var ptr = new IntPtr(filterConditions.ToInt64() + i * conditionSize);
                    Marshal.StructureToPtr(conditions[i], ptr, false);
                }

                filter.numFilterConditions = (uint)conditions.Length;
                filter.filterConditions = filterConditions;
            }

            var code = Methods.FwpmFilterAdd0(engineHandle, ref filter, IntPtr.Zero, out ulong id);
            if (code != 0)
                throw new NativeException(nameof(Methods.FwpmFilterAdd0), code);

            return filter;
        }
    }
}
