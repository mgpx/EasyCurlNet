﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyCurlNet.SafeHandles
{
    public sealed class SafeSocketHandle : SafeHandle
    {
        private SafeSocketHandle() : base(new IntPtr(-1), false)
        {
        }

        public override bool IsInvalid => handle == new IntPtr(-1);

        public static SafeSocketHandle Invalid = new IntPtr(-1);

        protected override bool ReleaseHandle()
        {
            throw new NotImplementedException();
        }

        public static implicit operator SafeSocketHandle(IntPtr ptr)
        {
            var handle = new SafeSocketHandle();
            handle.SetHandle(ptr);
            return handle;
        }
    }
}
