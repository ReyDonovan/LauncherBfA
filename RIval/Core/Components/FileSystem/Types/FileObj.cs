using RIval.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIval.Core.Components.FileSystem.Types
{
    public class FileObj : BaseRepository
    {
        public string NiceFileName { get; set; }
        public string FileName { get; set; }
        public string RemotePath { get; set; }

        public string Hash { get; set; }

        public FileObj() { }
    }
}
