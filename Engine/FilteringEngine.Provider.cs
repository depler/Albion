using Albion.Native;
using System;

namespace Albion.Engine
{
    public partial class FilteringEngine
    {
        public void AddProvider()
        {
            var provider = new FWPM_PROVIDER0();
            provider.providerKey = providerKey;
            provider.displayData.name = providerName;
            provider.flags = FWPM_PROVIDER_FLAG.PERSISTENT;

            var code = Methods.FwpmProviderAdd0(engineHandle, ref provider, IntPtr.Zero);
            if (code != 0 && code != (uint)FWP_E.ALREADY_EXISTS)
                throw new NativeException(nameof(Methods.FwpmProviderAdd0), code);
        }

        public void DeleteProvider()
        {
            var code = Methods.FwpmProviderDeleteByKey0(engineHandle, ref providerKey);
            if (code != 0 && code != (uint)FWP_E.PROVIDER_NOT_FOUND)
                throw new NativeException(nameof(Methods.FwpmProviderAdd0), code);
        }
    }
}
