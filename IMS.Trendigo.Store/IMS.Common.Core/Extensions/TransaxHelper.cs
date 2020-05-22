using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using IMS.Common.Core.Entities;
using IMS.Common.Core.Entities.Transax;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using IMS.Common.Core.Entities.IMS;
using IMS.Common.Core.Exceptions;
using IMS.Common.Core.Enumerations;
using AutoMapper;
using IMS.Common.Core.Data;
using System.Security.Cryptography;
using System.Globalization;

namespace IMS.Store.Common.Extensions
{
    public sealed class TransaxHelper
    {
        JSONHelper JHelp = new JSONHelper();

        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IMSEntities db = new IMSEntities();

        #region Crypto Section

        private static string patternDate = "MM/dd/yyyy";
        private static string patternHour = "H:mm";

        public static string GetSHA1(string str)
        {
            SHA1 sha = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static string GetSHA256(string str)
        {
            SHA256 sha = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static string GetHMACSHA256(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return ArrayToHexString(hashmessage);
            }
        }

        public static string ArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string GetSHA512(string str)
        {
            SHA512 sha = SHA512Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static string CreateUniqueToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string concat = Convert.ToBase64String(time.Concat(key).ToArray());
            string token = GetSHA1(concat);

            return token;
        }

        #endregion

        #region DateTime Section

        public static DateTime ConvertFromUnixTimestamp(Int64 timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static Int64 ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return (Int64)Math.Floor(diff.TotalSeconds);
        }

        public static Int64 ConvertStringToTimestamp(string date = "", string hour = "")
        {
            Int64 timestamp = 0;

            if (!(String.IsNullOrEmpty(date) || String.IsNullOrWhiteSpace(date)))
            {
                if (String.IsNullOrEmpty(hour) || String.IsNullOrWhiteSpace(hour))
                {
                    hour = "00:00";
                }

                DateTime parsedDate;
                string time = date + " " + hour;
                string pattern = patternDate + " " + patternHour;

                if (DateTime.TryParseExact(time, pattern, null, DateTimeStyles.None, out parsedDate))
                {
                    timestamp = ConvertToUnixTimestamp(parsedDate);
                }
            }

            return timestamp;
        }

        public static long DatetimeToTimestamp(DateTime dateTime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long timestamp = (long)dateTime.Subtract(sTime).TotalSeconds;
            return timestamp;
        }

        #endregion

        #region Authorization Section

        public String GetSessionKey(String userID, String privateKey, Boolean gotoEkoPay = true)
        {
            String TIMESTAMP = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();

            var sessionKeyObject = new TransaxHelper().GetSessionKey(userID, TIMESTAMP, privateKey, gotoEkoPay);

            return sessionKeyObject;
        }

        public TokenData GetToken(String userId, String privateKey, Boolean gotoEkoPay = true)
        {
            String sessionkey = GetSessionKey(userId, privateKey, gotoEkoPay);

            String TIMESTAMP = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();

            TokenData accessToken = new TransaxHelper().GetToken(sessionkey, TIMESTAMP, privateKey, gotoEkoPay);

            return accessToken;

        }

        /// <summary>
        /// This Method query the web API service to acquire the session key
        /// </summary>
        /// <param name="userID">This is the enterprise identification number</param>
        /// <param name="timestamp">This is the timestamp use in the query</param>
        /// <param name="privateKey">This is the private key associated with the enterprise</param>
        /// <returns>Session Key that will be used to acquire the token</returns>
        private String GetSessionKey(String userID = "", String timestamp = "", String privateKey = "", Boolean gotoEkoPay = true)
        {
            //X_PUBLIC string
            String sha_result_string = new HASH256Helper().getHashSha256(userID + timestamp);

            //DATA string
            String data = @"{""data"":""" + userID + @""",""timestamp"":""" + timestamp + @""",""hash"":""" + sha_result_string + @"""}";

            //X_HASH string
            string hmacHash = new HMACHelper().MyHMACHash(privateKey, data);
            String sessionKey = new TransaxHelper().GetSessionKey<String>(null, sha_result_string, hmacHash, data, gotoEkoPay);
            
            return sessionKey;
        }

        /// <summary>
        /// This Method query the web API service to acquire the token
        /// </summary>
        /// <param name="mySessionKey">This is the session key that was acquire by the enterprise</param>
        /// <param name="timestamp">This is the timestamp use in the query</param>
        /// <param name="privateKey">This is the private key associated with the enterprise</param>
        /// <returns></returns>
        private TokenData GetToken(String mySessionKey = "", String timestamp = "", String privateKey = "", Boolean gotoEkoPay = true)
        {
            //X_PUBLIC string
            String X_PUBLIC = new HASH256Helper().getHashSha256(mySessionKey + timestamp);

            //DATA string       WE NEED TO KNOW IF IT'S PERTINENT TO STORE A CLIENT TOKEN
            String DATA = @"{""data"":""" + mySessionKey + @""",""client_token"":""" + "1234567890" + @""",""timestamp"":""" + timestamp + @""",""session_key"":""" + mySessionKey + @""",""hash"":""" + X_PUBLIC + @"""}";

            //X_HASH string
            string X_HASH = new HMACHelper().MyHMACHash(privateKey, DATA);

            TokenData tokenData = new TransaxHelper().GetToken<TokenData>(null, X_PUBLIC, X_HASH, DATA, gotoEkoPay);
            return tokenData;
        }

        private T GetSessionKey<T>(object requestBodyObject, string publicHash = "", string privateHash = "", string data = "", bool gotoEkoPay = true)
        {
            string WebAPIaddress = gotoEkoPay == true ? ConfigurationManager.AppSettings["TransaxWebAPIAddress"] : ConfigurationManager.AppSettings["TrendigoWebAPIAddress"];

            return JHelp.CallRestJsonService<T>(WebAPIaddress + "sso/connect", requestBodyObject, "POST", publicHash, privateHash, data, "sessionkey");
        }

        private T GetToken<T>(object requestBodyObject, string publicHash = "", string privateHash = "", string data = "", bool gotoEkoPay = true)
        {
            string WebAPIaddress = gotoEkoPay == true ? ConfigurationManager.AppSettings["TransaxWebAPIAddress"] : ConfigurationManager.AppSettings["TrendigoWebAPIAddress"];

            return JHelp.CallRestJsonService<T>(WebAPIaddress + "sso/getTokens", requestBodyObject, "POST", publicHash, privateHash, data, "accesstoken");
        }

        #endregion

        #region Transaction Section

        /// <summary>
        /// Method that return the latest transactions of an enterprise from a specific datetime. 
        /// This will exclude the transaction from the date time provided in the parameter.
        /// </summary>
        /// <param name="token">This is for security validation</param>
        /// <param name="lastTransactionDateTime">DateTime of the last transaction</param>
        /// <param name="providerId">Id that represent the provider of the portal</param>
        /// <param name="enterpriseId">Id that represent the portal Id</param>
        /// <returns></returns>
        public async Task<TransaxTransactionRS> GetAllFinancialTransactions(TokenData token, String nextTransactionDateTime, String enterpriseId)
        {
            if (string.IsNullOrEmpty(enterpriseId) || string.IsNullOrEmpty(nextTransactionDateTime))
            {
                throw new IMSException(IMSCodeMessage.MissingParameter.ToString(), (int)IMSCodeMessage.MissingParameter);
            }

            var nvc = new Dictionary<string,string>();
            //nvc.Add("enterpriseId", enterpriseId);
            nvc.Add("sponsorId", enterpriseId);
            nvc.Add("systemDateTimeMin", nextTransactionDateTime);

            return await new TransaxHelper().GetAllFinancialTransactions<TransaxTransactionRS>(token, nvc, false);
        }

        public async Task<TransaxTransactionRS> GetAllFinancialTransactions(TokenData token, Int64 nextTransactionId, String enterpriseId)
        {
            if (string.IsNullOrEmpty(enterpriseId) || string.IsNullOrEmpty(nextTransactionId.ToString()))
            {
                throw new IMSException(IMSCodeMessage.MissingParameter.ToString(), (int)IMSCodeMessage.MissingParameter);
            }

            var nvc = new Dictionary<string, string>();
            nvc.Add("enterpriseId", enterpriseId);
            nvc.Add("idTransactionFinancial", nextTransactionId.ToString());

            return await new TransaxHelper().GetAllFinancialTransactions<TransaxTransactionRS>(token, nvc, false);
        }

        public async Task<TransaxTransactionRS> GetAllFinancialTransactions(TokenData token, String entityId, String terminalId, String DateTimeMin, String DateTimeMax, Boolean gotoEkoPay = true)
        {
            if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(terminalId) || string.IsNullOrEmpty(DateTimeMin) || string.IsNullOrEmpty(DateTimeMax))
            {
                throw new IMSException(IMSCodeMessage.MissingParameter.ToString(), (int)IMSCodeMessage.MissingParameter);
            }

            var nvc = new Dictionary<string, string>();
            nvc.Add("entityId", entityId);
            nvc.Add("systemDateTimeMin", DateTimeMin);
            nvc.Add("systemDateTimeMax", DateTimeMax);
            nvc.Add("trxTerminalId", terminalId);

            return await new TransaxHelper().GetAllFinancialTransactions<TransaxTransactionRS>(token, nvc, false, gotoEkoPay);
        }

        public async Task<TransaxNonFinancialTransactionRS> GetAllNonFinancialTransactions(TokenData token, String nextTransactionDateTime, String enterpriseId)
        {
            if (string.IsNullOrEmpty(enterpriseId) || string.IsNullOrEmpty(nextTransactionDateTime))
            {
                throw new IMSException(IMSCodeMessage.MissingParameter.ToString(), (int)IMSCodeMessage.MissingParameter);
            }

            var nvc = new Dictionary<string, string>();
            //nvc.Add("enterpriseId", enterpriseId);
            nvc.Add("sponsorId", enterpriseId);
            nvc.Add("dateServerMin", nextTransactionDateTime);

            return await new TransaxHelper().GetAllNonFinancialTransactions<TransaxNonFinancialTransactionRS>(token, nvc, false);
        }

        public async Task<TransaxNonFinancialTransactionRS> GetAllNonFinancialTransactions(TokenData token, Int64 nextTransactionId, String enterpriseId)
        {
            if (string.IsNullOrEmpty(enterpriseId) || string.IsNullOrEmpty(nextTransactionId.ToString()))
            {
                throw new IMSException(IMSCodeMessage.MissingParameter.ToString(), (int)IMSCodeMessage.MissingParameter);
            }

            var nvc = new Dictionary<string, string>();
            nvc.Add("enterpriseId", enterpriseId);
            nvc.Add("nonFinancialTransactionId", nextTransactionId.ToString());

            return await new TransaxHelper().GetAllNonFinancialTransactions<TransaxNonFinancialTransactionRS>(token, nvc, false);
        }

        private async Task<T> GetAllFinancialTransactions<T>(TokenData token, Dictionary<string, string> parameters = null, Boolean HandleMultipleValueKey = false, Boolean gotoEkoPay = true)
        {
            string WebAPIaddress = gotoEkoPay == true ? ConfigurationManager.AppSettings["TransaxWebAPIAddress"] : ConfigurationManager.AppSettings["TrendigoWebAPIAddress"];

            return await JHelp.CallRestJsonService<T>(WebAPIaddress + "transaction/getAll", parameters, false, token, HttpMethod.Post, "TransaxRS");
        }

        private async Task<T> GetAllNonFinancialTransactions<T>(TokenData token, Dictionary<string, string> parameters = null, Boolean HandleMultipleValueKey = false)
        {
            return await JHelp.CallRestJsonService<T>(ConfigurationManager.AppSettings["TransaxWebAPIAddress"] + "nonfinancialtransaction/getAll", parameters, false, token, HttpMethod.Post, "TransaxRS");
        }

        #endregion

        #region Credit Card Section

        public async Task<TransaxBatchCloseRS> ProcessBatchClosing(TokenData token, IMSEnterpriseParameterTerminal terminal)
        {
            if (string.IsNullOrEmpty(terminal.IMSEnterpriseParameter.MerchantId) || string.IsNullOrEmpty(terminal.IMSEnterpriseParameter.AcquirerId) || string.IsNullOrEmpty(terminal.Description))
            {
                throw new IMSException(IMSCodeMessage.MissingParameter);
            }

            if (string.IsNullOrEmpty(token.admin))
            {
                throw new IMSException(IMSCodeMessage.TokenNotProvided);
            }

            var nvc = new Dictionary<string, string>();
            nvc.Add("merchantId", terminal.IMSEnterpriseParameter.MerchantId.ToString());
            nvc.Add("terminalId", terminal.Description);
            nvc.Add("acquirerId", terminal.IMSEnterpriseParameter.AcquirerId.ToString());
            nvc.Add("language", "en");
            nvc.Add("type", ((int)IMS.Common.Core.Enumerations.TransaxTransactionType.BatchClose).ToString());

            var trxBatchCloseRS = await new TransaxHelper().ProcessBatchClosing<TransaxBatchCloseRS>(token, nvc, false);

            return trxBatchCloseRS;
        }

        private async Task<T> ProcessBatchClosing<T>(TokenData token, Dictionary<string, string> parameters = null, Boolean HandleMultipleValuekey = false) 
        {
            return await JHelp.CallRestJsonService<T>(ConfigurationManager.AppSettings["TransaxWebAPIAddress"] + "creditcard/Payment", parameters, false, token, HttpMethod.Post, "TransaxRS");
        }

        #endregion

    }
}