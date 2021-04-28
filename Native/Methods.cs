using System;
using System.Runtime.InteropServices;

namespace Albion.Native
{
    public static class Methods
    {
        [DllImport("FWPUCLNT.DLL")]
        public static extern void FwpmFreeMemory0(
            ref IntPtr p);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmEngineOpen0(
            [MarshalAs(UnmanagedType.LPWStr)] string serverName,
            uint authnService,
            IntPtr authIdentity,
            ref FWPM_SESSION0 session,
            out IntPtr engineHandle);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmEngineClose0(
            IntPtr engineHandle);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmProviderAdd0(
            IntPtr engineHandle,
            ref FWPM_PROVIDER0 provider,
            IntPtr sd);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmProviderGetByKey0(
            IntPtr engineHandle,
            ref Guid key,
            out IntPtr provider);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmProviderDeleteByKey0(
            IntPtr engineHandle,
            ref Guid key);

        [DllImport("FWPUCLNT.DLL")]
        internal static extern uint FwpmFilterAdd0(
            IntPtr engineHandle,
            ref FWPM_FILTER0 filter,
            IntPtr sd,
            out ulong id);

        [DllImport("FWPUCLNT.DLL")]
        internal static extern uint FwpmFilterDeleteByKey0(
            IntPtr engineHandle,
            ref Guid key);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmFilterCreateEnumHandle0(
            IntPtr engineHandle,
            IntPtr enumTemplate,
            out IntPtr enumHandle);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmFilterEnum0(
            IntPtr engineHandle,
            IntPtr enumHandle,
            uint numEntriesRequested,
            out IntPtr entries,
            out uint numEntriesReturned);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmFilterDestroyEnumHandle0(
            IntPtr engineHandle,
            IntPtr enumHandle);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmSubLayerAdd0(
            IntPtr engineHandle,
            ref FWPM_SUBLAYER0 subLayer,
            IntPtr sd);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmSubLayerDeleteByKey0(
            IntPtr engineHandle,
            ref Guid key);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmSubLayerCreateEnumHandle0(
            IntPtr engineHandle,
            IntPtr enumTemplate,
            out IntPtr enumHandle);

        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmSubLayerEnum0(
            IntPtr engineHandle,
            IntPtr enumHandle,
            uint numEntriesRequested,
            out IntPtr entries,
            out uint numEntriesReturned);


        [DllImport("FWPUCLNT.DLL")]
        public static extern uint FwpmSubLayerDestroyEnumHandle0(
            IntPtr engineHandle,
            IntPtr enumHandle);
    }
}
