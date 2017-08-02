using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace SharpRepository.CouchDbRepository
{
    public static class CouchDbRequest
    {
        public static string Execute(string url, string uri, HttpMethod method, string postData = null, string contentType = null)
        {
            //var req = WebRequest.Create(url) as HttpWebRequest;
            //req.Method = method;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
               
                if (contentType != null)
                {
                    client.DefaultRequestHeaders
                     .Accept
                     .Add(new MediaTypeWithQualityHeaderValue(contentType));
                }

                HttpRequestMessage request = new HttpRequestMessage(method, uri);

                if (postData != null)
                {
                    var bytes = Encoding.UTF8.GetBytes(postData);
                    request.Content = new ByteArrayContent(bytes);
                }

                try
                {
                    var resp = client.SendAsync(request).Result;
                    string result;
                    using (var reader = new StreamReader(resp.Content.ReadAsStreamAsync().Result))
                    {
                        result = reader.ReadToEnd();
                    }

                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
