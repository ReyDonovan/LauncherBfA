using RIval.Core.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIval.Core.Components.News
{
    public class NewsCache
    {
        private const string CACHE_STORAGE_PATH = "cache/news/";

        public List<NewsRepository> ActualNews { get; private set; } = new List<NewsRepository>();
        public NewsApiHandler       Api        { get; private set; }
        public DateTime             LastUpdate { get; private set; }

        public NewsCache() : this(new NewsApiHandler()) { }
        public NewsCache(NewsApiHandler handler)
        {
            Api = handler;

            Clear();

            if (!Directory.Exists(CACHE_STORAGE_PATH))
            {
                Directory.CreateDirectory(CACHE_STORAGE_PATH);
            }
        }

        public List<NewsRepository> Update()
        {
            if(DateTime.Compare(LastUpdate, DateTime.Now) < 0)
            {
                var result = Api.GetRemotedNews();
                result.Wait();

                if(result.IsCompleted)
                {
                    var tempNews = result.GetAwaiter().GetResult();
                    foreach(var element in tempNews)
                    {
                        string local = string.Empty;

                        if(!IsImageFinded(element.Image.Split('/').Last()))
                        {
                            var res = LoadImageToLocal(element.Image);
                            res.Wait();

                            if(res.IsCompleted)
                            {
                                local = res.GetAwaiter().GetResult();
                            }
                        }
                        else
                        {
                            local = CACHE_STORAGE_PATH + element.Image.Split('/').Last();
                        }

                        element.AddLocalImage(local);
                    }

                    ActualNews = tempNews;
                }

                LastUpdate = DateTime.Now;
            }
            else
            {
                Logger.Instance.WriteLine($"Trying to update news when time not expired! Result: {DateTime.Compare(LastUpdate, DateTime.Now)}", LogLevel.Warning);
            }

            return ActualNews;
        }
        public void Clear()
        {
            ActualNews.Clear();
            LastUpdate = DateTime.MinValue;
        }

        private Task<string> LoadImageToLocal(string remote)
        {
            return Task.Run(() =>
            {
                System.Net.WebClient client = new System.Net.WebClient();

                var localname = remote.Split('/').Last();
                client.DownloadFile(remote, $"{CACHE_STORAGE_PATH}\\{localname}");

                return CACHE_STORAGE_PATH + localname;
            });
        }
        private bool IsImageFinded(string local)
        {
            return File.Exists(CACHE_STORAGE_PATH + "\\" + local);
        }
    }
}
