using System;
using System.Collections.Generic;
using RestSharp;
using IMS.Utilities.PaymentAPI.Client;
using IMS.Utilities.PaymentAPI.Model;
using System.Threading.Tasks;

namespace IMS.Utilities.PaymentAPI.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ITransactionsApi
    {
        /// <summary>
        /// Search specific financial transactions. Searches for financial transactions based on search criteria.
        /// </summary>
        /// <param name="startIndex">The initial transaction number from which the result will be extracted.</param>
        /// <param name="maxResults">The number of results to return. Leave empty if you do not want to fix a limit (the maximum is set to 999).</param>
        /// <param name="enterpriseId">The enterprise Id that is linked to the transaction from which the result will be extracted.</param>
        /// <param name="transactionStatus">The transaction financial status.  Stored in the acquirer_response_message. Possible values are : ALL, APPROVED, DECLINED</param>
        /// <returns>List&lt;TransactionFinancial&gt;</returns>
        Task<List<TransactionFinancial>> FindFinancialTransactions(long? startIndex, int? maxResults, int? enterpriseId, string transactionStatus);
        /// <summary>
        /// Search specific non-financial transactions. Searches for non-financial transactions based on search criteria.
        /// </summary>
        /// <param name="startIndex">The initial non financial transaction number from which the result will be extracted.</param>
        /// <param name="maxResults">The number of results to return. Leave empty if you do not want to fix a limit (the maximum is set to 999).</param>
        /// <param name="enterpriseId">The enterprise Id that is linked to the non financial transaction from which the result will be extracted.</param>
        /// <returns>List&lt;TransactionNonFinancial&gt;</returns>
        Task<List<TransactionNonFinancial>> FindNonFinancialTransactions(long? startIndex, int? maxResults, int? enterpriseId);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class TransactionsApi : ITransactionsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public TransactionsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public TransactionsApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
        }

        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public String GetBasePath(String basePath)
        {
            return this.ApiClient.BasePath;
        }

        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient { get; set; }

        /// <summary>
        /// Search specific financial transactions. Searches for financial transactions based on search criteria.
        /// </summary>
        /// <param name="startIndex">The initial transaction number from which the result will be extracted.</param> 
        /// <param name="maxResults">The number of results to return. Leave empty if you do not want to fix a limit (the maximum is set to 999).</param> 
        /// <param name="enterpriseId">The enterprise Id that is linked to the transaction from which the result will be extracted.</param> 
        /// <param name="transactionStatus">The transaction financial status.  Stored in the acquirer_response_message. Possible values are : ALL, APPROVED, DECLINED</param> 
        /// <returns>List&lt;TransactionFinancial&gt;</returns>            
        public async Task<List<TransactionFinancial>> FindFinancialTransactions(long? startIndex, int? maxResults, int? enterpriseId, string transactionStatus)
        {

            // verify the required parameter 'startIndex' is set
            if (startIndex == null) throw new ApiException(400, "Missing required parameter 'startIndex' when calling FindFinancialTransactions");


            var path = "/transactions-financial/";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            if (startIndex != null) queryParams.Add("startIndex", ApiClient.ParameterToString(startIndex)); // query parameter
            if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
            if (enterpriseId != null) queryParams.Add("enterpriseId", ApiClient.ParameterToString(enterpriseId)); // query parameter
            if (transactionStatus != null) queryParams.Add("transactionStatus", ApiClient.ParameterToString(transactionStatus)); // query parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling FindFinancialTransactions: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindFinancialTransactions: " + response.ErrorMessage, response.ErrorMessage);

            return (List<TransactionFinancial>)ApiClient.Deserialize(response.Content, typeof(List<TransactionFinancial>), response.Headers);
        }

        /// <summary>
        /// Search specific non-financial transactions. Searches for non-financial transactions based on search criteria.
        /// </summary>
        /// <param name="startIndex">The initial non financial transaction number from which the result will be extracted.</param> 
        /// <param name="maxResults">The number of results to return. Leave empty if you do not want to fix a limit (the maximum is set to 999).</param> 
        /// <param name="enterpriseId">The enterprise Id that is linked to the non financial transaction from which the result will be extracted.</param> 
        /// <returns>List&lt;TransactionNonFinancial&gt;</returns>            
        public async Task<List<TransactionNonFinancial>> FindNonFinancialTransactions(long? startIndex, int? maxResults, int? enterpriseId)
        {

            // verify the required parameter 'startIndex' is set
            if (startIndex == null) throw new ApiException(400, "Missing required parameter 'startIndex' when calling FindNonFinancialTransactions");


            var path = "/transactions-non-financial/";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            if (startIndex != null) queryParams.Add("startIndex", ApiClient.ParameterToString(startIndex)); // query parameter
            if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
            if (enterpriseId != null) queryParams.Add("enterpriseId", ApiClient.ParameterToString(enterpriseId)); // query parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling FindNonFinancialTransactions: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindNonFinancialTransactions: " + response.ErrorMessage, response.ErrorMessage);

            return (List<TransactionNonFinancial>)ApiClient.Deserialize(response.Content, typeof(List<TransactionNonFinancial>), response.Headers);
        }

    }
}
