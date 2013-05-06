using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EpPingtree.Datalayer.Interfaces.Communication
{
    public interface IWebRequestRepository
    {
        int Timeout { get; set; }

        void EncodeKeyValue(StringWriter writer, ref bool isFirst, string key, string value);

        string PostRequest(string url, string requestBody, Dictionary<string, string> requestHeaders);
        string GetRequest(string url, Dictionary<string, string> requestKeys);
    }
}
