using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Headers = System.Collections.Generic.Dictionary<string, string>;

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
        private Headers RequestHeaders { get; set; }

        public ApiBuilder()
        {
            Response = new List<T>();
            Http = new HttpClient();
            RequestHeaders = new Headers();
        }

        public async Task<string> GetAsync(string uri)
        {
            if(RequestHeaders.Count > 0)
            {
                foreach (var header in RequestHeaders)
                    Http.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

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
                    if(RequestHeaders.Count > 0)
                    {
                        foreach (var header in RequestHeaders)
                            Http.DefaultRequestHeaders.Remove(header.Key);

                        RequestHeaders.Clear();
                    }

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
            }

            return this;
        }

        public ApiBuilder<T> AddHeader(string key, string val)
        {
            if(!RequestHeaders.ContainsKey(key))
            {
                RequestHeaders.Add(key, val);
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
