using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Web.Script.Serialization;

namespace IMS.Store.Common.Extensions
{
    public partial class XMLHelper
    {

        public T CallRestXMLService<T>(string uri, string method = "POST", string authorization = "")
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = 104857600; //200 MB unicode
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.Accept = "application/json";
            request.ContentType = "application/json; charset=utf-8";

            //This is for getting the session key
            
            request.Headers.Add("Authorization", "Basic " + authorization);

            var response = request.GetResponse();

            if (response == null)
            {
                return default(T);
            }

            //Read JSON response stream and deserialize
            var streamReader = new System.IO.StreamReader(response.GetResponseStream());
            var responseContent = streamReader.ReadToEnd().Trim();

            
            T jsonObject = javaScriptSerializer.Deserialize<T>(responseContent);
            return (T)Convert.ChangeType(jsonObject, typeof(T));
        }
    

        /// <summary>
        /// Deserializes an xml document back into an object
        /// </summary>
        /// <param name="xml">The xml data to deserialize</param>
        /// <param name="type">The type of the object being deserialized</param>
        /// <returns>A deserialized object</returns>
        public static object Deserialize(XmlDocument xml, Type type)
        {
            XmlSerializer s = new XmlSerializer(type);
            string xmlString = xml.OuterXml.ToString();
            byte[] buffer = ASCIIEncoding.UTF8.GetBytes(xmlString);
            MemoryStream ms = new MemoryStream(buffer);
            XmlReader reader = new XmlTextReader(ms);
            Exception caught = null;

            try
            {
                object o = s.Deserialize(reader);
                return o;
            }

            catch (Exception e)
            {
                caught = e;
            }
            finally
            {
                reader.Close();

                if (caught != null)
                    throw caught;
            }
            return null;
        }

        
    }
}
