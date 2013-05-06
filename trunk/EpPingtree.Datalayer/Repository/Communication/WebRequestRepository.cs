using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using EpPingtree.Datalayer.Interfaces.Communication;

namespace EpPingtree.Datalayer.Repository.Communication
{

    public class WebRequestRepository : BaseRepository, IWebRequestRepository
    {
        private class WebClientWithTimeout : WebClient
        {
            /// <summary>
            /// Timeout in milliseconds
            /// </summary>
            public int Timeout { get; set; }

            protected override WebRequest GetWebRequest(System.Uri address)
            {
                WebRequest webRequest = base.GetWebRequest(address);
                webRequest.Timeout = Timeout;
                return webRequest;
            }
        }

        /// <summary>
        /// Timeout in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public void EncodeKeyValue(StringWriter writer, ref bool isFirst, string key, string value)
        {
            if (!isFirst)
                writer.Write("&");

            isFirst = false;

            writer.Write(HttpUtility.UrlEncode(key));
            writer.Write("=");
            writer.Write(HttpUtility.UrlEncode(value));
        }

        public string PostRequest(string url, string requestBody, Dictionary<string, string> requestHeaders)
        {
            WebClientWithTimeout client = new WebClientWithTimeout();
            client.Timeout = Timeout;

            if (requestHeaders != null)
            {
                foreach (KeyValuePair<string, string> keys in requestHeaders)
                    client.Headers.Add(keys.Key, keys.Value);
            }

            if (url.StartsWith("https://"))
            {
                //Sending secure, ignore any SSL errors
                ServicePointManager.ServerCertificateValidationCallback = (obj, certificate, chain, errors) => true;
            }

            string response;

            //for integration tests
            if (url.Contains("fakesuccess"))
            {
                string successXML =
                    "<RESULT><CODE>1</CODE><MESSAGE>Application Processed</MESSAGE><URL><![CDATA[http://whatever.com/ApplyLead?lala=TEST]]></URL></RESULT>";
                response = successXML;

                return response;
            }

            if(url.Contains("fakerejected"))
            {
                string successXML =
                   "<RESULT><CODE>0</CODE><MESSAGE>Rejected - no partners accepted this lead</MESSAGE><URL></URL></RESULT>";
                response = successXML;

                return response;
            }

            response = client.UploadString(url, requestBody);
            

          

            return response;
        }

        /// <summary>
        /// Performs a Get Request and adds the get params as a query string
        /// </summary>
        public string GetRequest(string url, Dictionary<string, string> getParams)
        {
            StringBuilder stringBuilder = new StringBuilder(url);

            bool addSeperator = false;

            if (!url.Contains("?"))
            {
                stringBuilder.Append("?");
            }
            else
            {
                //The URL contains ?
                if (!url.EndsWith("?") && !url.EndsWith("&"))
                {
                    //The Url doesn't end with ? so already have query string params, add the ambersand to first param
                    addSeperator = true;
                }
            }

            foreach (KeyValuePair<string, string> keyValuePair in getParams)
            {
                if (addSeperator)
                    stringBuilder.Append("&");

                stringBuilder.Append(keyValuePair.Key);
                stringBuilder.Append("=");
                stringBuilder.Append(HttpUtility.UrlEncode(keyValuePair.Value));

                addSeperator = true;
            }

            url = stringBuilder.ToString();

            WebClient client = new WebClient();

            string response = client.DownloadString(url);
            return response;
        }
    }
}
