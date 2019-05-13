using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIval.Core.Components.Api
{
    public class ApiFacade : Singleton<ApiFacade>
    {
        private ApiUriTable Uri = new ApiUriTable();

        public string GetUri(string key)
        {
            return Uri.GetUri(key);
        }

        public ApiBuilder<T> Builder<T>()
        {
            return new ApiBuilder<T>();
        }
    }
}
