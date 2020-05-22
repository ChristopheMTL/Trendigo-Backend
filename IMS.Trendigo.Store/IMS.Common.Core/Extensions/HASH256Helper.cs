using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace IMS.Store.Common.Extensions
{
    public class HASH256Helper
    {
        /// <summary>
        /// This method return an Hash data with a Sha256 encryption
        /// </summary>
        /// <param name="text">The text to hash</param>
        /// <returns>The hash value</returns>
        public string getHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
    }
}
