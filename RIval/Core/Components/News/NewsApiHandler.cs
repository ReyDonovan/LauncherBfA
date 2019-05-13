using RIval.Core.Components.Api;
using RIval.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RIval.Core.Components.News
{
    public class NewsApiHandler
    {
        public Task<List<NewsRepository>> GetRemotedNews()
        {
            return Task.Run(() =>
            {
                return ApiFacade.Instance.Builder<NewsRepository>().CreateRequest(ApiFacade.Instance.GetUri("api-news-get")).GetResponse();
            });
        }
    }
}
