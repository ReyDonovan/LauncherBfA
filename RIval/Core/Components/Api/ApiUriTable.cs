using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Api
{
    public class ApiUriTable
    {
        public const string API_WWW = "http://crux.intellixservice.ru";

        private Dictionary<string, string> UrisTable = new Dictionary<string, string>()
        {
            ["api-news-get"] = "/api/articles/getall",
            ["api-update-check"] = "/api/update/check",
            ["api-update-get"] = "/api/update/get",
            ["api-update-util"] = "/api/update/utility",
            ["api-get-clientfiles-mini"] = "/api/client/mini/list",
            ["api-get-clientfiles-full"] = "/api/client/full/list",
            ["shop-link"] = "/ru-ru/shop/family/world-of-warcraft",
            ["acp-link"] = "/ru-ru/account/management",
            ["forum-link"] = "/ru-ru/forums",
            ["bug-report-link"] = "/community/bugs/report",
            ["api-user-login"] = "/api/user/login",
            ["api-user-register"] = "/api/user/register",
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
