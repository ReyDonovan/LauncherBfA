using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIval.Core.Components.Api
{
    public class ApiUriTable
    {
        public const string API_WWW = "http://crux.intellixservice.ru";

        private Dictionary<string, string> UrisTable = new Dictionary<string, string>()
        {
            ["api-news-get"] = "/api/articles/getall",
            ["api-update-check"] = "/update/check",
            ["api-update-get"] = "/update/get",
            ["api-update-util"] = "/update/utility",
            ["api-get-clientfiles-mini"] = "/api/client/mini/list",
            ["api-get-clientfiles-full"] = "/api/client/full/list",
            ["shop-link"] = "/shop",
            ["acp-link"] = "/acp",
            ["forum-link"] = "/community"
        };

        public string GetUri(string key)
        {
            if(UrisTable.ContainsKey(key))
            {
                return API_WWW + UrisTable[key];
            }
            else
            {
                return "";
            }
        }
    }
}
