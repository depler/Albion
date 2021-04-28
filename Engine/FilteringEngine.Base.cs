using Albion.Native;
using System;

namespace Albion.Engine
{
    public partial class FilteringEngine : IDisposable
    {
        private string providerName = "Albion Wall";
        private Guid providerKey = new Guid("d450cd12-023e-4c44-8c51-1a1e19cfb396");
        private IntPtr engineHandle = IntPtr.Zero;

        public FilteringEngine()
        {
            var session = new FWPM_SESSION0();
            session.sessionKey = Guid.NewGuid();
            session.flags = FWPM_SESSION_FLAG.NONE;

            var code = Methods.FwpmEngineOpen0(null, (uint)RPC_C_AUTHN.WINNT, IntPtr.Zero, ref session, out engineHandle);
            if (code != 0)
                throw new NativeException(nameof(Methods.FwpmEngineOpen0), code);
        }

        public void Dispose()
        {
            if (engineHandle != IntPtr.Zero)
            {
                var code = Methods.FwpmEngineClose0(engineHandle);
                if (code != 0)
                    throw new NativeException(nameof(Methods.FwpmEngineClose0), code);
            }
        }

        public void Initialize()
        {
            AddProvider();
            AddSubLayers();
        }

        public void Clear()
        {
            ClearFilters();
            ClearSubLayers();
            DeleteProvider();
        }
    }
}
