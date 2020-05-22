using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Exceptions
{
    public class TransaxException : Exception
    {
        private readonly string _statusCode;
        private readonly string _response;
        private readonly string _uri;
        private readonly IEnumerable<KeyValuePair<string, string>> _parameters = new Dictionary<string, string>();

        private TransaxException() { }

        public TransaxException(string statusCode, string response, string uri, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            _statusCode = statusCode;
            _response = response;
            _uri = uri;
            _parameters = parameters;
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.AppendFormat("Status code: \t{0}\r\n", _statusCode);
            b.AppendFormat("Response body:\t{0}\r\n", _response);
            b.AppendFormat("Request url: \t{0}\r\n", _uri);
            b.Append("Request parameters:\r\n");
            
            foreach (var kvp in _parameters)
            {
                b.AppendFormat("{0}= {1}\r\n", kvp.Key, kvp.Value);
            }
            b.Append("Exception details:\r\n");

            return b + base.ToString();            
        }
    }
}
