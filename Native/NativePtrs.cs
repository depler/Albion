using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Albion.Native
{
    public class NativePtrs : IDisposable
    {
        private List<IntPtr> ptrs = new List<IntPtr>();

        public void Add(IntPtr ptr)
        {
            ptrs.Add(ptr);
        }

        public IntPtr Add(int size)
        {
            var ptr = Marshal.AllocHGlobal(size);
            ptrs.Add(ptr);
            return ptr;
        }

        public IntPtr Add<T>(T value)
        {
            var ptr = Add(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(value, ptr, false);
            return ptr;
        }

        public void Dispose()
        {
            foreach (var pts in ptrs)
                Marshal.FreeHGlobal(pts);
        }
    }
}
