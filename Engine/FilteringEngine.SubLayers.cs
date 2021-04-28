using Albion.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Albion.Engine
{
    public partial class FilteringEngine
    {
        public IEnumerable<FWPM_SUBLAYER0> GetSubLayers()
        {
            var enumHandle = IntPtr.Zero;
            var entries = IntPtr.Zero;

            try
            {
                var code = Methods.FwpmSubLayerCreateEnumHandle0(engineHandle, IntPtr.Zero, out enumHandle);
                if (code != 0)
                    throw new NativeException(nameof(Methods.FwpmSubLayerCreateEnumHandle0), code);

                code = Methods.FwpmSubLayerEnum0(engineHandle, enumHandle, uint.MaxValue, out entries, out uint numEntriesReturned);
                if (code != 0)
                    throw new NativeException(nameof(Methods.FwpmSubLayerEnum0), code);

                var subLayerSize = Marshal.SizeOf<FWPM_SUBLAYER0>();
                for (uint i = 0; i < numEntriesReturned; i++)
                {
                    var ptr = new IntPtr(entries.ToInt64() + i * IntPtr.Size);
                    var ptr2 = Marshal.PtrToStructure<IntPtr>(ptr);
                    var subLayer = Marshal.PtrToStructure<FWPM_SUBLAYER0>(ptr2);

                    if (subLayer.providerKey == IntPtr.Zero)
                        continue;

                    var filterProviderKey = Marshal.PtrToStructure<Guid>(subLayer.providerKey);
                    if (filterProviderKey != providerKey)
                        continue;

                    yield return subLayer;
                }
            }
            finally
            {
                if (entries != IntPtr.Zero)
                    Methods.FwpmFreeMemory0(ref entries);

                if (enumHandle != IntPtr.Zero)
                {
                    var code = Methods.FwpmSubLayerDestroyEnumHandle0(engineHandle, enumHandle);
                    if (code != 0)
                        throw new NativeException(nameof(Methods.FwpmSubLayerDestroyEnumHandle0), code);
                }
            }
        }

        public void DeleteSubLayer(Guid key)
        {
            var code = Methods.FwpmSubLayerDeleteByKey0(engineHandle, ref key);
            if (code != 0 && code != (uint)FWP_E.SUBLAYER_NOT_FOUND)
                throw new NativeException(nameof(Methods.FwpmSubLayerDeleteByKey0), code);
        }

        public void ClearSubLayers()
        {
            foreach (var subLayer in GetSubLayers())
                DeleteSubLayer(subLayer.subLayerKey);
        }

        private void AddSubLayers()
        {
            using var ptrs = new NativePtrs();
            var provider = ptrs.Add(providerKey);

            foreach (var kv in SubLayers.All())
            {
                var subLayer = new FWPM_SUBLAYER0();
                subLayer.providerKey = provider;
                subLayer.subLayerKey = kv.Key;
                subLayer.displayData.name = kv.Value;
                subLayer.weight = ushort.MaxValue;
                subLayer.flags = FWPM_SUBLAYER_FLAG.PERSISTENT;

                var code = Methods.FwpmSubLayerAdd0(engineHandle, ref subLayer, IntPtr.Zero);
                if (code != 0 && code != (uint)FWP_E.ALREADY_EXISTS)
                    throw new NativeException(nameof(Methods.FwpmProviderAdd0), code);
            }
        }
    }
}
