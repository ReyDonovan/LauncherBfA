using Ignite.Core.Components.FileSystem.Types;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ignite.Core.Components.FileSystem.Additions
{
    public class FileChecker
    {
        public delegate void FileCheckerProcess(string filename, int percentage);
        public event FileCheckerProcess OnProcess;

        public void Subscribe(FileCheckerProcess handler)
        {
            try
            {
                if (!OnProcess.GetInvocationList().Contains(handler))
                {
                    OnProcess += handler;
                }
            }
            catch(NullReferenceException)
            {
                OnProcess += handler;
            }
        }

        public void Unsubscribe(FileCheckerProcess handler)
        {
            try
            {
                if (OnProcess.GetInvocationList().Contains(handler))
                {
                    OnProcess -= handler;
                }
            }
            catch (NullReferenceException) { }
        }

        public bool Check(FileObjectsCollection files)
        {
            string current = "";
            int currentFileChecked = 0;
            bool result = true;

            try
            {
                foreach (var item in files)
                {
                    current = item.NiceFileName;

                    if (item.NiceFileName.Length > 35)
                    {
                        current = current.Remove(35) + "...";
                    }

                    OnProcess(current, ((currentFileChecked * 100) / files.Count));

                    if (!File.Exists(item.FileName))
                    {
                        result = false;
                    }
                    else
                    {
                        if (!Hashinger.CompareHashRaw(item.FileName, item.Hash))
                        {
                            result = false;

                            File.Delete(item.FileName);
                        }
                    }

                    currentFileChecked++;
                }
            }
            catch (Exception ex)
            {
                ex.ToLog(LogLevel.Error);

                result = false;
            }

            return result;
        }

        public async Task<bool> CheckAsync(FileObjectsCollection files)
        {
            return await Task.Run(() => Check(files));
        }
    }
}
