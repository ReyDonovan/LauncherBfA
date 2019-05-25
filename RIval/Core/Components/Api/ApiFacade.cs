using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Api
{
    public class ApiFacade : Singleton<ApiFacade>
    {
        private ApiUriTable Uri = new ApiUriTable();

        public string GetUri(string key)
        {
            return Uri.GetUri(key);
        }

        public string BuildUri(string keyFromLib, params string[] pathes)
        {
            string uri = Uri.GetUri(keyFromLib);
            foreach(var item in pathes)
            {
                uri += $"/{item}";
            }

            return uri;
        }

        public ApiBuilder<T> Builder<T>()
        {
            return new ApiBuilder<T>();
        }
    }
}
