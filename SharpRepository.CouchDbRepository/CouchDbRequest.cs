using System.IO;
using System.Net;
using System.Text;

namespace SharpRepository.CouchDbRepository
{
    public static class CouchDbRequest
    {
        public static string Execute(string url, string method, string postdata=null, string contenttype=null)
        {
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = method;
            // Yuk - set an infinite timeout on this for now, because
            // executing a temporary view (for example) can take a very
            // long time...
            req.Timeout = System.Threading.Timeout.Infinite;
            if (contenttype != null)
                req.ContentType = contenttype;

            if (postdata != null)
            {
                var bytes = UTF8Encoding.UTF8.GetBytes(postdata.ToString());
                req.ContentLength = bytes.Length;
                using (var ps = req.GetRequestStream())
                {
                    ps.Write(bytes, 0, bytes.Length);
                }
            }

            var resp = req.GetResponse() as HttpWebResponse;
            string result;
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
