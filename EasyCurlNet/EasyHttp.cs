using EasyCurlNet.Enums;
using EasyCurlNet.Helpers;
using EasyCurlNet.SafeHandles;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace EasyCurlNet
{
    public class EasyHttp : IDisposable
    {
        private CURLcode mGlobal = CURLcode.FAILED_INIT;

        public String Url { get; set; } = String.Empty;
        public NameValueCollection Headers { get; set; } = new NameValueCollection();
        public EasyHttp() 
        {
            mGlobal = CurlNative.Init();
        }

        public EasyHttp(String url):this()
        {
            Url = url;
        }
        ~EasyHttp()
        {
            if (mGlobal == CURLcode.OK)
                CurlNative.Cleanup();
        }

        public void AddHeader(string name, string value)
        {
            Headers.Add(name, value);
        }

        public string Execute(String path, Method httpMethod, String body = null)
        {
            var easy = CurlNative.Easy.Init();

            var dataCopier = new DataCallbackCopier();

            CurlNative.Easy.SetOpt(easy, CURLoption.URL, UrlCombine(Url, path));
            CurlNative.Easy.SetOpt(easy, CURLoption.WRITEFUNCTION, dataCopier.DataHandler);

            //add headers
            SafeSlistHandle headers = CurlNative.Slist.Append(SafeSlistHandle.Null, "User-Agent: EasyCurlNet/1.0 (Windows)"); 
            foreach (var header in Headers)
                CurlNative.Slist.Append(headers, $"{header}: {Headers[(String)header]}");

            CurlNative.Easy.SetOpt(easy, CURLoption.HTTPHEADER, headers.DangerousGetHandle());
            CurlNative.Easy.SetOpt(easy, CURLoption.CUSTOMREQUEST, httpMethod.ToString().ToUpper());

            if (!String.IsNullOrEmpty(body))
                CurlNative.Easy.SetOpt(easy, CURLoption.COPYPOSTFIELDS, body);

            //CurlNative.Easy.SetOpt(easy, CURLoption.SSL_CIPHER_LIST, "ECDHE-RSA-AES256-GCM-SHA384:ECDHE-RSA-AES128-GCM-SHA256:TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256");
            //CurlNative.Easy.SetOpt(easy, CURLoption.SSLVERSION, 6);
            //string caCertPath = "curl-ca-bundle.crt"; 
            //CurlNative.Easy.SetOpt(easy, CURLoption.CAINFO, caCertPath);

            var strpem = ReadCertificate();
            var blob = new CurlNative.Easy.CurlBlob()
            {
                data = Marshal.StringToHGlobalAnsi(strpem),
                len = (UIntPtr)strpem.Length,
                flags = 1,
            };
            CurlNative.Easy.SetOpt(easy, CURLoption.CURLOPT_CAINFO_BLOB, blob);

            var result = CurlNative.Easy.Perform(easy);

            var resposta = Encoding.UTF8.GetString(dataCopier.Stream.ToArray());

            CurlNative.Slist.FreeAll(headers);
            Marshal.FreeHGlobal(blob.data);
            easy.Dispose();
            if (mGlobal == CURLcode.OK)
            {
                CurlNative.Cleanup();
            }

            return resposta;



        }

        private String UrlCombine(params string[] items)
        {
            if (items?.Any() != true)
            {
                return string.Empty;
            }
            return string.Join("/", items.Where(u => !string.IsNullOrWhiteSpace(u)).Select(u => u.Trim('/', '\\')));
        }

        private string ReadCertificate()
        {
            string file = "curl-ca-bundle.crt";
            string fullPath = "EasyCurlNet.Libs.Certificates." + file;

            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(fullPath))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return null;
        }

        public void Dispose()
        {
            if (mGlobal == CURLcode.OK)
            {
                CurlNative.Cleanup();
            }
        }


    }

    public enum Method
    {
        GET,
        POST,
        PUT,
        DELETE,
    }
}
