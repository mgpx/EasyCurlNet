using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCurlNet
{
    internal class Test
    {
        public static void Main()
        {
            var http = new EasyHttp("https://www.interativosistemas.com.br");
            using (http)
            {
                http.AddHeader("Content-Type", "application/json");
                http.Execute("licencas/webservice/licencas.php?act=true", Method.POST, null);
            }
            
        }
    }
}
