using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace Web.Api.Tests.WebRequest
{
    public class ClientHttp : IDisposable
    {
        public readonly string BaseUrlString;
        public readonly HttpServer Server;
        public bool IsDisposed { get; private set; }

        public ClientHttp(string baseUrlString)
        {
            if (String.IsNullOrEmpty(baseUrlString))
            {
                throw new NullReferenceException("baseUrlString is null at constructor.");
            }
            BaseUrlString = baseUrlString;
        }

        public ClientHttp(string baseUrlString, HttpServer server) : this(baseUrlString)
        {
            if (server == null)
            {
                throw new NullReferenceException("server is null at constructor.");
            }
            Server = server;
        }

        private Tuple<HttpClient, HttpRequestMessage> CreateRequest(string route, HttpMethod method)
        {
            HttpClient client = (Server != null) ? new HttpClient(Server) : new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (route == null)
            {
                throw new Exception("route shouldn't be null for http request");
            }
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(String.Format("{0}/{1}", BaseUrlString, route));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = method;

            return new Tuple<HttpClient, HttpRequestMessage>(client, request);
        }

        private Tuple<HttpClient, HttpRequestMessage> CreateRequest(string route, HttpMethod method, string dataString)
        {
            Tuple<HttpClient, HttpRequestMessage> request = CreateRequest(route, method);
            HttpRequestMessage requestMessage = request.Item2;
            requestMessage.Content = new StringContent(dataString, Encoding.UTF8, "application/json");
            return request;
        }

        private string BeginRequest(Tuple<HttpClient, HttpRequestMessage> request)
        {
            string result;

            HttpClient client = request.Item1;
            HttpRequestMessage requestMessage = request.Item2;
            using (client)
            {
                using (HttpResponseMessage response = client.SendAsync(requestMessage).Result)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpException((int) response.StatusCode, result);
                    }
                }
            }
            return result;
        }

        private TSource Deserialize<TSource>(string data)
        {
            return JsonConvert.DeserializeObject<TSource>(data);
        }

        private string Serialize<TSource>(TSource data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public string Get(string route)
        {
            return BeginRequest(CreateRequest(route, HttpMethod.Get));
        }

        public string Delete(string route)
        {
            return BeginRequest(CreateRequest(route, HttpMethod.Delete));
        }

        public string Post(string route, string dataString)
        {
            return BeginRequest(CreateRequest(route, HttpMethod.Post, dataString));
        }

        public string Put(string route, string dataString)
        {
            return BeginRequest(CreateRequest(route, HttpMethod.Put, dataString));
        }

        public TResponseData Get<TResponseData>(string route)
        {
            return Deserialize<TResponseData>(Get(route));
        }

        public TResponseData Delete<TResponseData>(string route)
        {
            return Deserialize<TResponseData>(Delete(route));
        }

        public TResponseData Post<TResponseData, TRequestData>(string route, TRequestData data)
        {
            return Deserialize<TResponseData>(Post(route, Serialize(data)));
        }

        public TResponseData Put<TResponseData, TRequestData>(string route, TRequestData data)
        {
            return Deserialize<TResponseData>(Put(route, Serialize(data)));
        }

        public void Dispose()
        {
            if (Server != null)
            {
                Server.Dispose();             
            }
            IsDisposed = true;
        }
    }
}
