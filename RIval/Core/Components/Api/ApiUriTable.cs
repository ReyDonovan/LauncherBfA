using System;
using System.Collections.Generic;

namespace Ignite.Core.Components.Api
{
    public class ApiUriTable
    {
        public const string API_WWW = "http://wowignite.ru";

        private Dictionary<string, string> UrisTable = new Dictionary<string, string>()
        {
            ["api-news-get"] = "/api/articles",
            ["api-update-check"] = "/api/update/check",
            ["api-update-get"] = "/api/update/get",
            ["api-update-util"] = "/api/update/utility",
            ["api-get-clientfiles-mini"] = "/api/client/mini/list",
            ["api-get-clientfiles-full"] = "/api/client/full/list",
            ["shop-link"] = "/shop/family/world-of-warcraft",
            ["acp-link"] = "/account/management",
            ["forum-link"] = "/forums",
            ["bug-report-link"] = "/forums/59",
            ["api-user-login"] = "/api/auth/login",
            ["api-user-register"] = "/api/auth/signup",
            ["api-user-getdata"] = "/api/auth/user",
        };

        public string GetUri(string key)
        {
            if(UrisTable.ContainsKey(key))
            {
                if (IsApiRoute(UrisTable[key]))
                    return API_WWW + UrisTable[key];
                else
                    return API_WWW + $"/{LanguageMgr.Instance.GetLangShort()}" + UrisTable[key];
            }
            else
            {
                return "";
            }
        }

        private bool IsApiRoute(string route)
        {
            return route.Contains("/api") || route.Contains("/loader");
        }
    }
}
