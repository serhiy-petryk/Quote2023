using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class WebClientExt : WebClient
    {
        public int? TimeoutInMilliseconds;
        public CookieContainer cookieContainer;
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);

            // request.Headers.Add(HttpRequestHeader.Connection, "keep-alive");
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Referer = address.Host;
            if (cookieContainer != null)
                request.CookieContainer = cookieContainer;

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.AllowAutoRedirect = true;

            if (TimeoutInMilliseconds.HasValue)
                request.Timeout = TimeoutInMilliseconds.Value;
            return request;
        }

    }
}
