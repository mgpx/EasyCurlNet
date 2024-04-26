using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyCurlNet.SafeHandles
{
    public sealed class SafeMultiHandle : SafeHandle
    {
        private SafeMultiHandle() : base(IntPtr.Zero, false)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            CurlNative.Multi.Cleanup(handle);
            return true;
        }
    }
}
