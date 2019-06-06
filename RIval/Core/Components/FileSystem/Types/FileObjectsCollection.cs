using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.FileSystem.Types
{
    public class FileObjectsCollection : List<FileObj>
    {
        public const string UNKNOWN_PATH = "upath";

        public string GetFilename(int id)
        {
            var ret = FindByID(id);
            if (ret != null)
            {
                return ret.FileName;
            }

            return UNKNOWN_PATH;
        }
        public string GetFilename(string remPath)
        {
            var ret = FindByPathes(remPath);
            if (ret != null)
            {
                return ret.FileName;
            }

            return UNKNOWN_PATH;
        }

        public string GetRemotePath(int id)
        {
            var ret = FindByID(id);
            if (ret != null)
            {
                return ret.RemotePath;
            }

            return UNKNOWN_PATH;
        }
        public string GetRemotePath(string localPath)
        {
            var ret = FindByPathes(localPath);
            if(ret != null)
            {
                return ret.RemotePath;
            }

            return UNKNOWN_PATH;
        }

        private FileObj FindByID(int id)
        {
            return this.FirstOrDefault((element) => element.Id == id);
        }

        private FileObj FindByPathes(string pattern)
        {
            return this.FirstOrDefault((element) => element.FileName.Contains(pattern) || element.RemotePath.Contains(pattern));
        }
    }
}
