using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace IgniteUpdater
{
    public class ApiBuilder<T>
    {
        private List<T> Response { get; set; }

        public ApiBuilder()
        {
            Response = new List<T>();
        }

        public async Task<string> GetAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public ApiBuilder<T> CreateRequest(string uri)
        {
            if (uri == "")
            {
                return this;
            }

            try
            {
                var result = GetAsync(uri);
                result.Wait();

                var response = JsonConvert.DeserializeObject<T[]>(result.GetAwaiter().GetResult());

                foreach (var item in response)
                {
                    Response.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return this;
        }

        public ApiBuilder<T> DownloadTo(string fullpath, Action<FileStream, string> callback)
        {
            try
            {
                WebClient client = new WebClient();

                Task t = new Task(() =>
                {
                    client.DownloadFile(new Uri((string)(object)Response.First()), fullpath);
                });

                t.Start();
                Task.WaitAll(t);

                using (FileStream stream = File.OpenWrite(fullpath))
                {
                    callback(stream, fullpath);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return this;
        }

        public List<T> GetResponse()
        {
            return Response;
        }

        public ApiBuilder<T> AppendOne(int index, Action<T> method)
        {
            if(Response.Count > index)
            {
                method(Response[index]);
            }

            return this;
        }
        public ApiBuilder<T> AppendAll(Action<T> method)
        {
            foreach(var res in Response)
            {
                method(res);
            }

            return this;
        }
        public ApiBuilder<T> AppendAllAsync(Action<T> method)
        {
            var task = new Task(() =>
            {
                foreach(var res in Response)
                {
                    method(res);
                }
            });

            task.Start();
            task.Wait();

            return this;
        }
    }
}
