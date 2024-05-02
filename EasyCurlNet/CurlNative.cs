using EasyCurlNet.Enums;
using EasyCurlNet.SafeHandles;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EasyCurlNet
{
    public static class CurlNative
    {
        static CurlNative()
        {
            //https://stackoverflow.com/a/31115656/6163123
            //var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, IntPtr.Size == 8 ? "x64" : "x86");
            bool ok = SetDllDirectory(path);
            if (!ok) throw new System.ComponentModel.Win32Exception();
        }
        //private const string LIBCURL = "libcurl-x64.dll";
        private const string LIBCURL = "libcurl.dll";

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDllDirectory(string path);

        [DllImport(LIBCURL, EntryPoint = "curl_global_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode Init(CURLglobal flags = CURLglobal.DEFAULT);

        [DllImport(LIBCURL, EntryPoint = "curl_global_cleanup", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Cleanup();

        public static class Easy
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate UIntPtr DataHandler(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr userdata);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_init", CallingConvention = CallingConvention.Cdecl)]
            public static extern SafeEasyHandle Init();

            [DllImport(LIBCURL, EntryPoint = "curl_easy_cleanup", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Cleanup(IntPtr handle);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_perform", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLcode Perform(SafeEasyHandle handle);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_reset", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Reset(SafeEasyHandle handle);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_setopt", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLcode SetOpt(SafeEasyHandle handle, CURLoption option, int value);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_setopt", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLcode SetOpt(SafeEasyHandle handle, CURLoption option, IntPtr value);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_setopt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLcode SetOpt(SafeEasyHandle handle, CURLoption option, string value);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_setopt", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLcode SetOpt(SafeEasyHandle handle, CURLoption option, DataHandler value);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_setopt", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLcode SetOpt(SafeEasyHandle handle, CURLoption option, CurlBlob value);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_getinfo", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLcode GetInfo(SafeEasyHandle handle, CURLINFO option, out int value);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_getinfo", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLcode GetInfo(SafeEasyHandle handle, CURLINFO option, out IntPtr value);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_getinfo", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLcode GetInfo(SafeEasyHandle handle, CURLINFO option, out double value);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_getinfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLcode GetInfo(SafeEasyHandle handle, CURLINFO option, IntPtr value);

            [DllImport(LIBCURL, EntryPoint = "curl_easy_strerror", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr StrError(CURLcode errornum);

            [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 0, Size = 16)]
            public struct CurlBlob
            {
                //com LPStr, LPWStr, LPTStr, BStr ou ByValTStr).'

                //public IntPtr data;
                [MarshalAs(UnmanagedType.SysInt, SizeConst = 8)]
                [FieldOffset(4)]
                public IntPtr data;
                [FieldOffset(8)]
                [MarshalAs(UnmanagedType.U4)]
                public uint len;
                [FieldOffset(12)]
                [MarshalAs(UnmanagedType.U4)]
                public int flags;

                /* Flag bits in the curl_blob struct: */
                public const uint CURL_BLOB_COPY = 1; /* tell libcurl to copy the data */
                public const uint CURL_BLOB_NOCOPY = 0; /* tell libcurl to NOT copy the data */
            }

            //public struct CurlBlob
            //{
            //    //public IntPtr data;
            //    [FieldOffset(0)]
            //    public IntPtr data1;
            //    [FieldOffset(4)]

            //    public IntPtr data;
            //    [FieldOffset(8)]
            //    [MarshalAs(UnmanagedType.U4)]
            //    public UIntPtr len;
            //    [MarshalAs(UnmanagedType.U4)]
            //    [FieldOffset(12)]
            //    public int flags;

            //    /* Flag bits in the curl_blob struct: */
            //    public const uint CURL_BLOB_COPY = 1; /* tell libcurl to copy the data */
            //    public const uint CURL_BLOB_NOCOPY = 0; /* tell libcurl to NOT copy the data */
            //}
            //public struct CurlBlob
            //{
            //    /*https://github.com/curl/curl/blob/9fc4b2c78c5a2619e1423e56a98b503d363c085f/include/curl/easy.h#L34*/
            //    /* Flag bits in the curl_blob struct: */
            //    public const uint CURL_BLOB_COPY = 1; /* tell libcurl to copy the data */
            //    public const uint CURL_BLOB_NOCOPY = 0; /* tell libcurl to NOT copy the data */

            //    public IntPtr data;
            //    public UIntPtr len;
            //    public uint flags;
            //}
        }

        public static class Multi
        {
            [DllImport(LIBCURL, EntryPoint = "curl_multi_init", CallingConvention = CallingConvention.Cdecl)]
            public static extern SafeMultiHandle Init();

            [DllImport(LIBCURL, EntryPoint = "curl_multi_cleanup", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLMcode Cleanup(IntPtr multiHandle);

            [DllImport(LIBCURL, EntryPoint = "curl_multi_add_handle", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLMcode AddHandle(SafeMultiHandle multiHandle, SafeEasyHandle easyHandle);

            [DllImport(LIBCURL, EntryPoint = "curl_multi_remove_handle", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLMcode RemoveHandle(SafeMultiHandle multiHandle, SafeEasyHandle easyHandle);

            [DllImport(LIBCURL, EntryPoint = "curl_multi_setopt", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLMcode SetOpt(SafeMultiHandle multiHandle, CURLMoption option, int value);

            [DllImport(LIBCURL, EntryPoint = "curl_multi_info_read", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr InfoRead(SafeMultiHandle multiHandle, out int msgsInQueue);

            [DllImport(LIBCURL, EntryPoint = "curl_multi_socket_action", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLMcode SocketAction(SafeMultiHandle multiHandle, SafeSocketHandle sockfd,
                CURLcselect evBitmask,
                out int runningHandles);

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct CURLMsg
            {
                public CURLMSG msg; /* what this message means */
                public IntPtr easy_handle; /* the handle it concerns */
                public CURLMsgData data;

                [StructLayout(LayoutKind.Explicit)]
                public struct CURLMsgData
                {
                    [FieldOffset(0)] public IntPtr whatever; /* (void*) message-specific data */
                    [FieldOffset(0)] public CURLcode result; /* return code for transfer */
                }
            }

            #region curl_multi_setopt for CURLMOPT_TIMERFUNCTION

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate int TimerCallback(IntPtr multiHandle, int timeoutMs, IntPtr userp);

            [DllImport(LIBCURL, EntryPoint = "curl_multi_setopt", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLMcode SetOpt(SafeMultiHandle multiHandle, CURLMoption option, TimerCallback value);

            #endregion

            #region curl_multi_setopt for CURLMOPT_SOCKETFUNCTION

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate int SocketCallback(IntPtr easy, IntPtr s, CURLpoll what, IntPtr userp, IntPtr socketp);

            [DllImport(LIBCURL, EntryPoint = "curl_multi_setopt", CallingConvention = CallingConvention.Cdecl)]
            public static extern CURLMcode SetOpt(SafeMultiHandle multiHandle, CURLMoption option,
                SocketCallback value);

            #endregion
        }

        public static class Slist
        {
            [DllImport(LIBCURL, EntryPoint = "curl_slist_append", CallingConvention = CallingConvention.Cdecl)]
            public static extern SafeSlistHandle Append(SafeSlistHandle slist, string data);

            [DllImport(LIBCURL, EntryPoint = "curl_slist_free_all", CallingConvention = CallingConvention.Cdecl)]
            public static extern void FreeAll(SafeSlistHandle pList);
        }
    }
}
