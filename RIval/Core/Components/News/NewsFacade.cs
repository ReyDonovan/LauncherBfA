using Ignite.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.News
{
    public class NewsFacade : Singleton<NewsFacade>
    {
        private NewsApiHandler Api { get; set; } = new NewsApiHandler();
        private NewsCache Cache;

        public NewsFacade()
        {
            Cache = new NewsCache(Api);
        }

        public List<NewsRepository> LoadNews()
        {
            return Cache.Update();
        }
    }
}
