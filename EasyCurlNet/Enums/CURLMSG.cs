using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCurlNet.Enums
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum CURLMSG
    {
        /// <summary>
        ///     First, not used.
        /// </summary>
        NONE,

        /// <summary>
        ///     This easy handle has completed. 'result' contains the CURLcode of the transfer.
        /// </summary>
        DONE,

        /// <summary>
        ///     Last, not used.
        /// </summary>
        LAST
    }
}
