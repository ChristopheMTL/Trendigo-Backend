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
    public interface IMerchantsApi
    {
        /// <summary>
        /// Create a merchant. Create a merchant.
        /// </summary>
        /// <param name="merchant"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateMerchant(Merchant merchant);
        /// <summary>
        /// Create a merchantProcessor. Create a merchant-processor relation.
        /// </summary>
        /// <param name="merchantProcessor"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateMerchantProcessor(MerchantProcessor merchantProcessor);
        /// <summary>
        /// Delete a specific merchant. Delete a merchant with a given identifier. It is not possible to delete a merchant if there are still ACTIVE locations for this merchant.
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task DeleteMerchant(int? merchantId);
        /// <summary>
        /// Delete a specific merchantProcessor. Delete a merchantProcessor with a given identifier.
        /// </summary>
        /// <param name="merchantProcessorId"></param>
        /// <returns></returns>
        Task DeleteMerchantProcessor(int? merchantProcessorId);
        /// <summary>
        /// Find a specific merchant. Find a merchant with a given identifier.
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns>Merchant</returns>
        Task<Merchant> FindMerchant(int? merchantId);
        /// <summary>
        /// Find the merchantProcessor. Find the merchant Processor with the merchant processor Id.
        /// </summary>
        /// <param name="merchantProcessorId"></param>
        /// <returns>MerchantProcessor</returns>
        Task<MerchantProcessor> FindMerchantProcessor(int? merchantProcessorId);
        /// <summary>
        /// Find the merchantProcessors specific to a merchant. Find all the merchantProcessors specific to a merchant with a given identifier.
        /// </summary>
        /// <param name="merchantId">The merchant associated to the merchant processor.</param>
        /// <returns>List&lt;MerchantProcessor&gt;</returns>
        Task<List<MerchantProcessor>> FindMerchantProcessors(int? merchantId);
        /// <summary>
        /// Search specific merchants matching a given pattern. Searches for merchants (merchantName, displayName, email) that match a certain pattern. Use a wildcard character (&#39;*&#39;) to match any number of characters.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param>
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param>
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param>
        /// <returns>List&lt;Merchant&gt;</returns>
        Task<List<Merchant>> FindMerchants(string filter, long? startIndex, int? maxResults);
        /// <summary>
        /// Update a specific merchant. Update a merchant with a given identifier.
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="merchant"></param>
        /// <returns></returns>
        Task UpdateMerchant(int? merchantId, Merchant merchant);
        /// <summary>
        /// Update a specific merchantProcessor. Update a merchantProcessor.
        /// </summary>
        /// <param name="merchantProcessorId"></param>
        /// <param name="merchantProcessor"></param>
        /// <returns></returns>
        Task UpdateMerchantProcessor(int? merchantProcessorId, MerchantProcessor merchantProcessor);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class MerchantsApi : IMerchantsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public MerchantsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MerchantsApi(String basePath)
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
        /// Create a merchant. Create a merchant.
        /// </summary>
        /// <param name="merchant"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateMerchant(Merchant merchant)
        {

            // verify the required parameter 'merchant' is set
            if (merchant == null) throw new ApiException(400, "Missing required parameter 'merchant' when calling CreateMerchant");


            var path = "/merchants";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(merchant); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateMerchant: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateMerchant: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Create a merchantProcessor. Create a merchant-processor relation.
        /// </summary>
        /// <param name="merchantProcessor"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateMerchantProcessor(MerchantProcessor merchantProcessor)
        {

            // verify the required parameter 'merchantProcessor' is set
            if (merchantProcessor == null) throw new ApiException(400, "Missing required parameter 'merchantProcessor' when calling CreateMerchantProcessor");


            var path = "/merchants/processors/";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(merchantProcessor); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateMerchantProcessor: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateMerchantProcessor: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Delete a specific merchant. Delete a merchant with a given identifier. It is not possible to delete a merchant if there are still ACTIVE locations for this merchant.
        /// </summary>
        /// <param name="merchantId"></param> 
        /// <returns></returns>            
        public async Task DeleteMerchant(int? merchantId)
        {

            // verify the required parameter 'merchantId' is set
            if (merchantId == null) throw new ApiException(400, "Missing required parameter 'merchantId' when calling DeleteMerchant");


            var path = "/merchants/{merchantId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "merchantId" + "}", ApiClient.ParameterToString(merchantId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;


            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteMerchant: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteMerchant: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Delete a specific merchantProcessor. Delete a merchantProcessor with a given identifier.
        /// </summary>
        /// <param name="merchantProcessorId"></param> 
        /// <returns></returns>            
        public async Task DeleteMerchantProcessor(int? merchantProcessorId)
        {

            // verify the required parameter 'merchantProcessorId' is set
            if (merchantProcessorId == null) throw new ApiException(400, "Missing required parameter 'merchantProcessorId' when calling DeleteMerchantProcessor");


            var path = "/merchants/processors/{merchantProcessorId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "merchantProcessorId" + "}", ApiClient.ParameterToString(merchantProcessorId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;


            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteMerchantProcessor: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteMerchantProcessor: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Find a specific merchant. Find a merchant with a given identifier.
        /// </summary>
        /// <param name="merchantId"></param> 
        /// <returns>Merchant</returns>            
        public async Task<Merchant> FindMerchant(int? merchantId)
        {

            // verify the required parameter 'merchantId' is set
            if (merchantId == null) throw new ApiException(400, "Missing required parameter 'merchantId' when calling FindMerchant");


            var path = "/merchants/{merchantId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "merchantId" + "}", ApiClient.ParameterToString(merchantId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;


            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling FindMerchant: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindMerchant: " + response.ErrorMessage, response.ErrorMessage);

            return (Merchant)ApiClient.Deserialize(response.Content, typeof(Merchant), response.Headers);
        }

        /// <summary>
        /// Find the merchantProcessor. Find the merchant Processor with the merchant processor Id.
        /// </summary>
        /// <param name="merchantProcessorId"></param> 
        /// <returns>MerchantProcessor</returns>            
        public async Task<MerchantProcessor> FindMerchantProcessor(int? merchantProcessorId)
        {

            // verify the required parameter 'merchantProcessorId' is set
            if (merchantProcessorId == null) throw new ApiException(400, "Missing required parameter 'merchantProcessorId' when calling FindMerchantProcessor");


            var path = "/merchants/processors/{merchantProcessorId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "merchantProcessorId" + "}", ApiClient.ParameterToString(merchantProcessorId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;


            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling FindMerchantProcessor: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindMerchantProcessor: " + response.ErrorMessage, response.ErrorMessage);

            return (MerchantProcessor)ApiClient.Deserialize(response.Content, typeof(MerchantProcessor), response.Headers);
        }

        /// <summary>
        /// Find the merchantProcessors specific to a merchant. Find all the merchantProcessors specific to a merchant with a given identifier.
        /// </summary>
        /// <param name="merchantId">The merchant associated to the merchant processor.</param> 
        /// <returns>List&lt;MerchantProcessor&gt;</returns>            
        public async Task<List<MerchantProcessor>> FindMerchantProcessors(int? merchantId)
        {


            var path = "/merchants/processors/";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            if (merchantId != null) queryParams.Add("merchantId", ApiClient.ParameterToString(merchantId)); // query parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling FindMerchantProcessors: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindMerchantProcessors: " + response.ErrorMessage, response.ErrorMessage);

            return (List<MerchantProcessor>)ApiClient.Deserialize(response.Content, typeof(List<MerchantProcessor>), response.Headers);
        }

        /// <summary>
        /// Search specific merchants matching a given pattern. Searches for merchants (merchantName, displayName, email) that match a certain pattern. Use a wildcard character (&#39;*&#39;) to match any number of characters.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param> 
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param> 
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param> 
        /// <returns>List&lt;Merchant&gt;</returns>            
        public async Task<List<Merchant>> FindMerchants(string filter, long? startIndex, int? maxResults)
        {


            var path = "/merchants";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            if (filter != null) queryParams.Add("filter", ApiClient.ParameterToString(filter)); // query parameter
            if (startIndex != null) queryParams.Add("startIndex", ApiClient.ParameterToString(startIndex)); // query parameter
            if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling FindMerchants: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindMerchants: " + response.ErrorMessage, response.ErrorMessage);

            return (List<Merchant>)ApiClient.Deserialize(response.Content, typeof(List<Merchant>), response.Headers);
        }

        /// <summary>
        /// Update a specific merchant. Update a merchant with a given identifier.
        /// </summary>
        /// <param name="merchantId"></param> 
        /// <param name="merchant"></param> 
        /// <returns></returns>            
        public async Task UpdateMerchant(int? merchantId, Merchant merchant)
        {

            // verify the required parameter 'merchantId' is set
            if (merchantId == null) throw new ApiException(400, "Missing required parameter 'merchantId' when calling UpdateMerchant");

            // verify the required parameter 'merchant' is set
            if (merchant == null) throw new ApiException(400, "Missing required parameter 'merchant' when calling UpdateMerchant");


            var path = "/merchants/{merchantId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "merchantId" + "}", ApiClient.ParameterToString(merchantId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(merchant); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateMerchant: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateMerchant: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Update a specific merchantProcessor. Update a merchantProcessor.
        /// </summary>
        /// <param name="merchantProcessorId"></param> 
        /// <param name="merchantProcessor"></param> 
        /// <returns></returns>            
        public async Task UpdateMerchantProcessor(int? merchantProcessorId, MerchantProcessor merchantProcessor)
        {

            // verify the required parameter 'merchantProcessorId' is set
            if (merchantProcessorId == null) throw new ApiException(400, "Missing required parameter 'merchantProcessorId' when calling UpdateMerchantProcessor");

            // verify the required parameter 'merchantProcessor' is set
            if (merchantProcessor == null) throw new ApiException(400, "Missing required parameter 'merchantProcessor' when calling UpdateMerchantProcessor");


            var path = "/merchants/processors/{merchantProcessorId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "merchantProcessorId" + "}", ApiClient.ParameterToString(merchantProcessorId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(merchantProcessor); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateMerchantProcessor: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateMerchantProcessor: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

    }
}
