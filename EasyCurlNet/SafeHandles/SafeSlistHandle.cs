using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyCurlNet.SafeHandles
{
    public sealed class SafeSlistHandle : SafeHandle
    {
        private SafeSlistHandle() : base(IntPtr.Zero, false)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        public static SafeSlistHandle Null => new SafeSlistHandle();

        protected override bool ReleaseHandle()
        {
            CurlNative.Slist.FreeAll(this);
            return true;
        }
    }
}
