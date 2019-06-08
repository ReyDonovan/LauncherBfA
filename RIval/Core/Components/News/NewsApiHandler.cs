using Ignite.Core.Components.Api;
using Ignite.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ignite.Core.Components.News
{
    public class NewsApiHandler
    {
        public Task<List<NewsRepository>> GetRemotedNews()
        {
            return Task.Run(() =>
            {
                return ApiFacade.Instance.Builder<NewsRepository>().CreateRequest(ApiFacade.Instance.GetUri("api-news-get") + $"/{LanguageMgr.Instance.GetLangShort()}").GetResponse();
            });
        }
    }
}
