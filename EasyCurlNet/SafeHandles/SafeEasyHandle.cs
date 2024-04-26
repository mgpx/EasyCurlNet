﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyCurlNet.SafeHandles
{
    public sealed class SafeEasyHandle : SafeHandle
    {
        private SafeEasyHandle() : base(IntPtr.Zero, false)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            CurlNative.Easy.Cleanup(handle);
            return true;
        }
    }
}
