using Albion.Code;
using Albion.Native;
using System;
using System.Collections.Generic;

namespace Albion.Engine
{
    public partial class FilteringEngine
    {
        public FWPM_FILTER_CONDITION0 CreateConditionProtocol(IPPROTO protocol)
        {
            var condition = new FWPM_FILTER_CONDITION0();
            condition.matchType = FWP_MATCH.EQUAL;
            condition.fieldKey = ConditionKeys.FWPM_CONDITION_IP_PROTOCOL;
            condition.conditionValue.type = FWP_DATA_TYPE.UINT8;
            condition.conditionValue.value.uint8 = (byte)protocol;
            return condition;
        }

        public FWPM_FILTER_CONDITION0 CreateConditionPort(bool remote, ushort port)
        {
            var condition = new FWPM_FILTER_CONDITION0();
            condition.matchType = FWP_MATCH.EQUAL;
            condition.fieldKey = remote ? ConditionKeys.FWPM_CONDITION_IP_REMOTE_PORT : ConditionKeys.FWPM_CONDITION_IP_LOCAL_PORT;
            condition.conditionValue.type = FWP_DATA_TYPE.UINT16;
            condition.conditionValue.value.uint16 = (ushort)port;
            return condition;
        }

        public FWPM_FILTER_CONDITION0 CreateConditionSubnetV4(bool remote, uint addr, uint mask, NativePtrs ptrs)
        {
            var addr4 = new FWP_V4_ADDR_AND_MASK();
            addr4.addr = addr;
            addr4.mask = mask;

            var condition = new FWPM_FILTER_CONDITION0();
            condition.matchType = FWP_MATCH.EQUAL;
            condition.fieldKey = remote ? ConditionKeys.FWPM_CONDITION_IP_REMOTE_ADDRESS : ConditionKeys.FWPM_CONDITION_IP_LOCAL_ADDRESS;
            condition.conditionValue.type = FWP_DATA_TYPE.V4_ADDR_MASK;
            condition.conditionValue.value.v4AddrMask = ptrs.Add(addr4);
            return condition;
        }

        public FWPM_FILTER_CONDITION0 CreateConditionSubnetV6(bool remote, byte[] addr, byte prefix, NativePtrs ptrs)
        {
            var addr6 = new FWP_V6_ADDR_AND_MASK();
            addr6.addr = addr;
            addr6.prefixLength = prefix;

            var condition = new FWPM_FILTER_CONDITION0();
            condition.matchType = FWP_MATCH.EQUAL;
            condition.fieldKey = remote ? ConditionKeys.FWPM_CONDITION_IP_REMOTE_ADDRESS : ConditionKeys.FWPM_CONDITION_IP_LOCAL_ADDRESS;
            condition.conditionValue.type = FWP_DATA_TYPE.V4_ADDR_MASK;
            condition.conditionValue.value.v4AddrMask = ptrs.Add(addr6);
            return condition;
        }

        public IEnumerable<FWPM_FILTER_CONDITION0> CreateConditionsProtocol(IPPROTO[] values)
        {
            if (values == null)
                yield break;

            foreach (var value in values)
                yield return CreateConditionProtocol(value);
        }

        public IEnumerable<FWPM_FILTER_CONDITION0> CreateConditionsPortOrSubnet(bool v6, bool remote, string[] values, NativePtrs ptrs)
        {
            if (values == null)
                yield break;

            foreach (var value in values)
            {
                if (ushort.TryParse(value, out ushort port))
                    yield return CreateConditionPort(false, port);
                else if (!v6 && Utils.TryParseSubnetV4(value, out uint addr4, out uint mask4))
                    yield return CreateConditionSubnetV4(false, addr4, mask4, ptrs);
                else if (v6 && Utils.TryParseSubnetV6(value, out byte[] addr6, out byte prefix6))
                    yield return CreateConditionSubnetV6(false, addr6, prefix6, ptrs);
                else 
                    throw new ArgumentOutOfRangeException(nameof(values));
            }
        }
    }
}
