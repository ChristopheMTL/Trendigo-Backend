using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using IMS.Common.Core.Entities;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using IMS.Common.Core.Exceptions;
using IMS.Common.Core.Entities.Transax;
using IMS.Common.Core.Services;
using System.Runtime.Serialization.Json;
using IMS.Common.Core.Slack;
using IMS.Common.Core.Enumerations;

namespace IMS.Store.Common.Extensions
{
    public class JSONHelper
    {

        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Call JSON/REST web service with basic authentication
        /// </summary>
        /// <typeparam name="T">The type to seserialize response to and return</typeparam>
        /// <param name="uri">Uri of the web service method</param>
        /// <param name="parameters">Parameter collection that has to be pass in the body of the request</param>
        /// <param name="handleMultipleValuesPerKey">Set if the key can allow multiple value parameters</param>
        /// <param name="token">This is the security token</param>
        /// <param name="method">HTTP method/verb, HttpMethod.Post or "GET"</param>
        /// <param name="rootElementName">This is the root element of the xml response that we want to deserialize to, this will replace the general root element in the response</param>
        /// <returns></returns>
        public async Task<T> CallRestJsonService<T>(string uri, IEnumerable<KeyValuePair<string, string>> parameters, Boolean handleMultipleValuesPerKey, HttpMethod method)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                //If it's a GET, we need to add the parameter on the query string
                if (parameters != null && method == HttpMethod.Get)
                {
                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                    foreach (var kvp in parameters)
                    {
                        query[kvp.Key] = kvp.Value;
                    }

                    uriBuilder.Query = query.ToString();
                }

                HttpRequestMessage request = new HttpRequestMessage(method, uriBuilder.ToString());

                //If it's a POST, we need to add the parameters in the body
                if (parameters != null && method == HttpMethod.Post)
                {
                    request.Content = new FormUrlEncodedContent(parameters);
                }

                var response = await client.SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();

                //_logger.Debug(uri);
                //_logger.Debug(responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        //TODO: handle specific transax error codes here
                        default:
                            _logger.Debug(uri);
                            _logger.Debug(responseContent);
                            throw new Exception(response.StatusCode.ToString() + responseContent + uri + parameters);
                    }
                }

                T xmlObject;

                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.IsNullable = true;

                var serializer = new XmlSerializer(typeof(T), xRoot);

                using (TextReader reader = new StringReader(responseContent))
                {
                    xmlObject = (T)serializer.Deserialize(reader);
                }

                return (T)Convert.ChangeType(xmlObject, typeof(T));
            }
        }

        /// <summary>
        /// Call JSON/REST web service with basic authentication
        /// </summary>
        /// <typeparam name="T">The type to seserialize response to and return</typeparam>
        /// <param name="uri">Uri of the web service method</param>
        /// <param name="requestBodyObject">Object to serialize and pass to web service method</param>
        /// <param name="method">HTTP method/verb, HttpMethod.Post or "GET"</param>
        /// <param name="username">Optional username if basic authentication is to be used</param>
        /// <param name="password">Optional password if basic authentication password is used</param>
        /// <returns></returns>
        [Obsolete]
        public T CallRestJsonService<T>(string uri, object requestBodyObject, string method, string publicHash, string privateHash, string data, string requestType = "normal")
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof (T)) ;   //new
            //javaScriptSerializer.MaxJsonLength = 104857600; //200 MB unicode
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.Accept = "application/json";
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = 0;

            //This is for getting the session key
            if (!string.IsNullOrEmpty(publicHash) && !string.IsNullOrEmpty(privateHash) && !string.IsNullOrEmpty(data))
            {
                request.Headers.Add("HTTP_X_PUBLIC", publicHash);
                request.Headers.Add("HTTP_X_HASH", privateHash);
                request.Headers.Add("HTTP_DATA", data);
            }

            //Serialize request object as JSON and write to request body
            if (requestBodyObject != null)
            {
                var stringBuilder = new StringBuilder();
                javaScriptSerializer.Serialize(requestBodyObject, stringBuilder);
                //System.Xml.XmlWriter xmlWriter = new System.Xml.xmlTextWriter(); //new
                //xmlSerializer.Serialize(xmlWriter, requestBodyObject);  //new
                var requestBody = stringBuilder.ToString();
                request.ContentLength = requestBody.Length;
                var streamWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                streamWriter.Write(requestBody);
                //streamWriter.Write(xmlWriter.ToString());   //new
                streamWriter.Close();
            }

            try
            {
                WebResponse response = request.GetResponse();

                //Read JSON response stream and deserialize
                var streamReader = new System.IO.StreamReader(response.GetResponseStream());
                var responseContent = streamReader.ReadToEnd().Trim();

                if (requestType.ToLower() == "sessionkey") //Session Key
                {
                    SessionKeyData jsonObject = javaScriptSerializer.Deserialize<SessionKeyData>(responseContent);
                    //SessionKeyData xmlObject = xmlSerializer.Deserialize(response.GetResponseStream()) as SessionKeyData;   //new
                    return (T) Convert.ChangeType(jsonObject.session_key, typeof (T));
                    //return (T)Convert.ChangeType(xmlObject.session_key, typeof(T));     //new
                }
                else if (requestType.ToLower() == "accesstoken") //Access Token
                {
                    TokenData jsonObject = javaScriptSerializer.Deserialize<TokenData>(responseContent);
                    //TokenData xmlObject = xmlSerializer.Deserialize(response.GetResponseStream()) as TokenData;   //new
                    return (T) Convert.ChangeType(jsonObject, typeof (T));
                    //return (T)Convert.ChangeType(xmlObject, typeof(T));   //new
                }
                else
                {
                    T jsonObject = javaScriptSerializer.Deserialize<T>(responseContent);
                    //T jsonObject = (T) xmlSerializer.Deserialize(response.GetResponseStream());     //new
                    return (T) Convert.ChangeType(jsonObject, typeof (T));
                }
            }
            catch (WebException ex)
            {
                _logger.ErrorFormat("method: {0}\r\n", method);
                _logger.ErrorFormat("requestType: {0}\r\n", requestType);
                _logger.ErrorFormat("publicHash: {0}\r\n", publicHash);
                _logger.ErrorFormat("privateHash: {0}\r\n", privateHash);
                _logger.ErrorFormat("data: {0}\r\n", data);
                
                _logger.Error(ex);

                new SlackClient().SlackAlert((int)SlackChannelEnum.WebAPI, ex.ToString());
                
                //var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                Stream stream = ex.Response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var jsonObject = serializer.DeserializeObject(sr.ReadToEnd());

                return (T) Convert.ChangeType(jsonObject, typeof (T));
            }
            catch (Exception ex2)
            {
                _logger.ErrorFormat("method: {0}\r\n", method);
                _logger.ErrorFormat("requestType: {0}\r\n", requestType);
                _logger.ErrorFormat("publicHash: {0}\r\n", publicHash);
                _logger.ErrorFormat("privateHash: {0}\r\n", privateHash);
                _logger.ErrorFormat("data: {0}\r\n", data);

                _logger.Error(ex2);

                throw;
            }
        }

        /// <summary>
        /// Call JSON/REST web service with basic authentication
        /// </summary>
        /// <typeparam name="T">The type to seserialize response to and return</typeparam>
        /// <param name="uri">Uri of the web service method</param>
        /// <param name="parameters">Parameter collection that has to be pass in the body of the request</param>
        /// <param name="handleMultipleValuesPerKey">Set if the key can allow multiple value parameters</param>
        /// <param name="token">This is the security token</param>
        /// <param name="method">HTTP method/verb, HttpMethod.Post or "GET"</param>
        /// <param name="rootElementName">This is the root element of the xml response that we want to deserialize to, this will replace the general root element in the response</param>
        /// <returns></returns>
        public async Task<T> CallRestJsonService<T>(string uri, IEnumerable<KeyValuePair<string, string>> parameters, Boolean handleMultipleValuesPerKey, TokenData token, HttpMethod method, string rootElementName)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            string accessToken = Token_ConvertToBase64(token.access_token);


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                // Auth
                if (!string.IsNullOrEmpty(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", accessToken);
                }

                //If it's a GET, we need to add the parameter on the query string
                if (parameters != null && method == HttpMethod.Get)
                {
                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                    foreach (var kvp in parameters)
                    {
                        query[kvp.Key] = kvp.Value;
                    }

                    uriBuilder.Query = query.ToString();
                }

                HttpRequestMessage request = new HttpRequestMessage(method, uriBuilder.ToString());

                //If it's a POST, we need to add the parameters in the body
                if (parameters != null && method == HttpMethod.Post)
                {
                    request.Content = new FormUrlEncodedContent(parameters);
                }

                var response = await client.SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        //TODO: handle specific transax error codes here
                        default:
                            _logger.Debug(uri);
                            _logger.Debug(responseContent);
                            throw new TransaxException(response.StatusCode.ToString(), responseContent, uri, parameters);
                    }
                }

                //We have to replace the general root element for the root we want to deserialize the reponse to
                responseContent = responseContent.Replace("<xml>", "<" + rootElementName + ">");
                responseContent = responseContent.Replace("</xml>", "</" + rootElementName + ">");
                responseContent = responseContent.Replace("<xml", "<" + rootElementName);

                T xmlObject;

                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = rootElementName;
                xRoot.IsNullable = true;

                var serializer = new XmlSerializer(typeof (T), xRoot);

                using (TextReader reader = new StringReader(responseContent))
                {
                    xmlObject = (T) serializer.Deserialize(reader);
                }

                return (T) Convert.ChangeType(xmlObject, typeof (T));

            }
        }

        public TimeZoneModel GetTimeZoneName(string uri, Dictionary<string, string> parameters) 
        {
            UriBuilder uriBuilder = new UriBuilder(uri);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                using (var content = new MultipartFormDataContent())
                {
                    if (parameters != null)
                    {
                        uri = uri + "?";

                        foreach (var kvp in parameters)
                        {
                            uri += kvp.Key.ToString() + "=" + kvp.Value.ToString() + "&";
                        }

                        uri = uri.Substring(0, uri.Length -1);
                    }

                    var response = client.PostAsync(uri, null);

                    var responseContent = response.Result.Content.ReadAsStringAsync();

                    var ds = new DataContractJsonSerializer(typeof(TimeZoneModel));
                    var msnew = new MemoryStream(Encoding.UTF8.GetBytes(responseContent.Result));
                    TimeZoneModel tzm = (TimeZoneModel)ds.ReadObject(msnew);

                    return tzm;
                }
            }
        }

        public async Task<T> UploadImage<T>(string uri, Dictionary<string, string> parameters, byte[] image, string imageName, TokenData token, string rootElementName)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            string accessToken = Token_ConvertToBase64(token.access_token);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                // Auth
                if (!string.IsNullOrEmpty(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", accessToken);
                }

                using (var content = new MultipartFormDataContent())
                {
                    if (parameters != null)
                    {
                        foreach (var kvp in parameters)
                        {
                            content.Add(new StringContent(kvp.Value.ToString()), kvp.Key.ToString());
                        }
                    }
                    var t = new StreamContent(new MemoryStream(image));
                    t.Headers.ContentType
                        = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                    content.Add(t, "uploadedfile", "" + imageName + "");

                    var response = await client.PostAsync(uri, content);

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        switch (response.StatusCode)
                        {
                            //TODO: handle specific transax error codes here
                            default:
                                throw new IMSException("Transax request returned status code " + response.StatusCode + "\r\n" + responseContent);
                        }
                    }

                    //We have to replace the general root element for the root we want to deserialize the reponse to
                    responseContent = responseContent.Replace("<xml>", "<" + rootElementName + ">");
                    responseContent = responseContent.Replace("</xml>", "</" + rootElementName + ">");
                    responseContent = responseContent.Replace("<xml", "<" + rootElementName);

                    T xmlObject;

                    XmlRootAttribute xRoot = new XmlRootAttribute();
                    xRoot.IsNullable = true;

                    var serializer = new XmlSerializer(typeof(T), xRoot);

                    using (TextReader reader = new StringReader(responseContent))
                    {
                        xmlObject = (T)serializer.Deserialize(reader);
                    }

                    return (T)Convert.ChangeType(xmlObject, typeof(T));
                }
            }
        }

        static Dictionary<string, object> NvcToDictionary(NameValueCollection nvc, bool handleMultipleValuesPerKey)
        {
            var result = new Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = nvc.GetValues(key);
                    if (values.Length == 1)
                    {
                        result.Add(key, values[0]);
                    }
                    else
                    {
                        result.Add(key, values);
                    }
                }
                else
                {
                    result.Add(key, nvc[key]);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TokenData"></param>
        /// <returns></returns>
        private string Token_ConvertToBase64(string TokenData)
        {
            var bytes = Encoding.UTF8.GetBytes("user:" + TokenData);
            var Token_base64 = Convert.ToBase64String(bytes);
            return Token_base64;
        }
    }
}
