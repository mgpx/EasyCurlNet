using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCurlNet.Enums
{
    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum CURLcselect
    {
        IN = 0x01,
        OUT = 0x02,
        ERR = 0x04
    }
}
