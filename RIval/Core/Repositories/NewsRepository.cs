using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Repositories
{
    public class NewsRepository : BaseRepository
    { 
        public string Title   { get; set; }
        public string Description { get; set; }
        public string Link    { get; set; }
        public string Image { get; set; }
        public string LocalImage { get; set; }

        public NewsRepository() { }
        public NewsRepository(int id, string title, string description, string image)
        {
            Id = id;
            Title   = title;
            Description = description;
            Link    = "http://wowignite.ru/ru-ru/news/" + Id;
            Image = image;
        }

        public void AddLocalImage(string path)
        {
            LocalImage = path;
        }
    }
}
