using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Net.Http;

namespace Ignite.Core.Components.Api
{
    public enum RequestMethod
    {
        GET,
        POST
    }
    public class ApiBuilder<T>
    {
        private List<T> Response { get; set; }
        private HttpClient Http { get; set; }

        public ApiBuilder()
        {
            Response = new List<T>();
            Http = new HttpClient();
        }

        public async Task<string> GetAsync(string uri)
        {
            return await Http.GetAsync(uri).GetAwaiter().GetResult().Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string uri, Dictionary<string, string> body)
        {
            var content = new FormUrlEncodedContent(body);

            return await Http.PostAsync(uri, content).GetAwaiter().GetResult().Content.ReadAsStringAsync();
        }

        public ApiBuilder<T> CreateRequest(string uri, RequestMethod method = RequestMethod.GET, Dictionary<string, string> body = null)
        {
            if (uri == "")
            {
                Logger.Instance.WriteLine("Incorrect URI for run API request.", LogLevel.Error);

                return this;
            }

            try
            {
                Task<string> awaiter;

                if (method == RequestMethod.GET)
                {
                    awaiter = GetAsync(uri);
                    awaiter.Wait();
                }
                else
                {
                    awaiter = PostAsync(uri, body);
                    awaiter.Wait();
                }

                try
                {
                    Logger.Instance.WriteLine(awaiter.GetAwaiter().GetResult(), LogLevel.Warning);

                    var response = JsonConvert.DeserializeObject<T[]>(awaiter.GetAwaiter().GetResult());

                    foreach (var item in response)
                    {
                        Response.Add(item);
                    }
                }
                catch(Exception)
                {
                    var response = JsonConvert.DeserializeObject<T>(awaiter.GetAwaiter().GetResult());

                    Response.Add(response);
                }
            }
            catch(Exception ex)
            {
                //ex.Report("UNKNOWN WEB ERROR");
                ex.ToLog(LogLevel.Error);

                MessageBox.Show("Server connection error. Please try again later", "Connection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return this;
        }

        public ApiBuilder<T> DownloadTo(string fullpath, Action<FileStream, string> callback)
        {
            try
            {
                FileMgr.Instance.CreateDirectory(fullpath.Replace(fullpath.Split('\\').Last(), ""));

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
                ex.ToLog(LogLevel.Error);
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
