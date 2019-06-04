using Ignite.Core.Components.Api;
using System.Linq;

namespace Ignite.Core.Components.Update
{
    public class UpdateApiHandler
    {
        public string GetRemoteVersionStr()
        {
            return ApiFacade.Instance.Builder<string>().CreateRequest(ApiFacade.Instance.GetUri("api-update-check")).GetResponse().First();
        }

        public string GetUpdaterUri()
        {
            return ApiFacade.Instance.Builder<string>().CreateRequest(ApiFacade.Instance.GetUri("api-update-util")).GetResponse().First();
        }
    }
}
