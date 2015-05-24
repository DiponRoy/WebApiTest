using System;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Web.Api.Sdk.Core
{
    public class JsonApi
    {
        public readonly string BaseUrl;
        public bool NotFound { get; protected set; }
        public bool ServerError { get; protected set; }

        public JsonApi(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        protected virtual T MakeCall<T>(string apiPath)
        {
            var baseUrl = new Uri(BaseUrl);
            var url = new Uri(baseUrl, apiPath);
            try
            {
                JObject data = MakeCall(wc => wc.DownloadString(url));
                return JsonConvert.DeserializeObject<T>(data.ToString());
            }
            catch (Exception ex)
            {
                throw new SdkException("Unable to load api at url: " + url, ex);
            }
        }


        protected JObject MakeCall(string apiPath)
        {
            var baseUrl = new Uri(BaseUrl);
            var url = new Uri(baseUrl, apiPath);
            try
            {
                return MakeCall(wc => wc.DownloadString(url));
            }
            catch (Exception ex)
            {
                throw new SdkException("Unable to load api at url: " + url, ex);
            }
        }

        private JObject MakeCall(Func<WebClient, string> executeFunc)
        {
            using (var wc = new WebClient())
            {
                // Allow domains we don't have a certificate for
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                SetHeaders(wc);

                string responseBody = null;

                try
                {
                    responseBody = executeFunc.Invoke(wc);
                    if (!responseBody.StartsWith("{"))
                    {
                        responseBody = string.Format("{{data:{0}}}", responseBody);
                    }
                }
                catch (WebException wex)
                {
                    Parse404(wex);
                }

                if (responseBody != null)
                {
                    JObject o = JObject.Parse(responseBody);
                    return o;
                }

                return null;
            }
        }

        /*Add additional heraders if needed*/

        private void SetHeaders(WebClient wc)
        {
            //wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + _bearerToken;
            //AddHeader(wc, "Authorization", "Bearer " + _bearerToken);
            //AddHeader(wc, "X-Developer-Id", _developerId);
            //AddHeader(wc, "X-Api-Key", _apiKey);
        }

        private void AddHeader(WebClient wc, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                wc.Headers[name] = value;
            }
        }

        private void Parse404(WebException wex)
        {
            var response = wex.Response as HttpWebResponse;
            if (response == null)
            {
                throw new SdkException("", wex);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Debug.WriteLine("Four, oh Four...");
                NotFound = true;
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                Debug.WriteLine("Piper down!");
                ServerError = true;
                throw new SdkException(wex.Message);
            }
        }

        protected virtual JObject Put(string apiPath, object data)
        {
            return UploadData(apiPath, "PUT", data);
        }
        protected virtual JObject Delete(string apiPath, object data)
        {
            return UploadData(apiPath, "DELETE", data);
        }

        protected virtual JObject Post(string apiPath, object data)
        {
            return UploadData(apiPath, "POST", data);
        }

        private JObject UploadData(string apiPath, string method, object data)
        {
            var baseUrl = new Uri(BaseUrl);
            var url = new Uri(baseUrl, apiPath);
            try
            {
                return MakeCall(wc =>
                {
                    // Allow domains we don't have a certificate for
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    return wc.UploadString(url, method, JsonConvert.SerializeObject(data));
                });
            }
            catch (Exception ex)
            {
                throw new SdkException("Unable to load api at url: " + url, ex);
            }
        }
    }
}