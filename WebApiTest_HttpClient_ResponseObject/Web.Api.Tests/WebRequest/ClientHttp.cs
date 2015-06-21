using System;
using System.Collections.Generic;
using System.Linq;
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
        public readonly Dictionary<string, string> Headers; 
        public readonly HttpServer Server;
        public bool IsDisposed { get; private set; }

        /*impt: to call on every instance*/
        public ClientHttp(string baseUrlString)
        {
            if (String.IsNullOrEmpty(baseUrlString))
            {
                throw new NullReferenceException("baseUrlString is null at constructor.");
            }
            BaseUrlString = baseUrlString;
            Headers = new Dictionary<string, string>();
        }

        public ClientHttp(string baseUrlString, HttpServer server) : this(baseUrlString)
        {
            if (server == null)
            {
                throw new NullReferenceException("server is null at constructor.");
            }
            Server = server;
        }

        private void SetHeadersIfAny(HttpClient client)
        {
            if (Headers.Any())
            {
                foreach (var header in Headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        /*impt: to create each request*/
        private Tuple<HttpClient, HttpRequestMessage> Request(string route, HttpMethod method)
        {
            if (route == null)
            {
                throw new Exception("route shouldn't be null for http request");
            }

            //create client
            HttpClient client = (Server != null) ? new HttpClient(Server) : new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //add headers
            SetHeadersIfAny(client);

            //request body
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(String.Format("{0}/{1}", BaseUrlString, route));
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Method = method;

            return new Tuple<HttpClient, HttpRequestMessage>(client, requestMessage);
        }

        private Tuple<HttpClient, HttpRequestMessage> Request(string route, HttpMethod method, string dataString)
        {
            Tuple<HttpClient, HttpRequestMessage> request = Request(route, method);
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
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpException((int) response.StatusCode, "Error");    //need to get actual error message
                    }
                    /*
                     * don't use that to produce error message, 
                     * some time error like HttpResponseException makes it null, and error again when trying to read
                     */
                    result = response.Content.ReadAsStringAsync().Result;   
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
            return BeginRequest(Request(route, HttpMethod.Get));
        }

        public string Delete(string route)
        {
            return BeginRequest(Request(route, HttpMethod.Delete));
        }

        public string Post(string route, string dataString)
        {
            return BeginRequest(Request(route, HttpMethod.Post, dataString));
        }

        public string Put(string route, string dataString)
        {
            return BeginRequest(Request(route, HttpMethod.Put, dataString));
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
