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
    public interface IPromotionsApi
    {
        /// <summary>
        /// Create a new promotion for a specific merchant. Create a new promotion for a specific merchant.
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreatePromotion(Promotion promotion);
        /// <summary>
        /// Delete a specific promotion. Delete a promotion for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        Task DeletePromotion(int? promotionId);
        /// <summary>
        /// Find a specific promotion. Find a promotion for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns>Promotion</returns>
        Task<Promotion> FindPromotion(int? promotionId);
        /// <summary>
        /// Find the promotions. Find all the promotions.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param>
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param>
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param>
        /// <returns>List&lt;Promotion&gt;</returns>
        Task<List<Promotion>> FindPromotions(string filter, long? startIndex, int? maxResults);
        /// <summary>
        /// Update a promotion. Update a promotion for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="promotionId"></param>
        /// <param name="promotion"></param>
        /// <returns></returns>
        Task UpdatePromotion(int? promotionId, Promotion promotion);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class PromotionsApi : IPromotionsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public PromotionsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public PromotionsApi(String basePath)
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
        /// Create a new promotion for a specific merchant. Create a new promotion for a specific merchant.
        /// </summary>
        /// <param name="promotion"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreatePromotion(Promotion promotion)
        {

            // verify the required parameter 'promotion' is set
            if (promotion == null) throw new ApiException(400, "Missing required parameter 'promotion' when calling CreatePromotion");


            var path = "/promotions";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(promotion); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreatePromotion: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreatePromotion: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Delete a specific promotion. Delete a promotion for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="promotionId"></param> 
        /// <returns></returns>            
        public async Task DeletePromotion(int? promotionId)
        {

            // verify the required parameter 'promotionId' is set
            if (promotionId == null) throw new ApiException(400, "Missing required parameter 'promotionId' when calling DeletePromotion");


            var path = "/promotions/{promotionId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "promotionId" + "}", ApiClient.ParameterToString(promotionId));

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
                throw new ApiException((int)response.StatusCode, "Error calling DeletePromotion: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeletePromotion: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Find a specific promotion. Find a promotion for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="promotionId"></param> 
        /// <returns>Promotion</returns>            
        public async Task<Promotion> FindPromotion(int? promotionId)
        {

            // verify the required parameter 'promotionId' is set
            if (promotionId == null) throw new ApiException(400, "Missing required parameter 'promotionId' when calling FindPromotion");


            var path = "/promotions/{promotionId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "promotionId" + "}", ApiClient.ParameterToString(promotionId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindPromotion: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindPromotion: " + response.ErrorMessage, response.ErrorMessage);

            return (Promotion)ApiClient.Deserialize(response.Content, typeof(Promotion), response.Headers);
        }

        /// <summary>
        /// Find the promotions. Find all the promotions.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param> 
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param> 
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param> 
        /// <returns>List&lt;Promotion&gt;</returns>            
        public async Task<List<Promotion>> FindPromotions(string filter, long? startIndex, int? maxResults)
        {


            var path = "/promotions";
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
                throw new ApiException((int)response.StatusCode, "Error calling FindPromotions: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindPromotions: " + response.ErrorMessage, response.ErrorMessage);

            return (List<Promotion>)ApiClient.Deserialize(response.Content, typeof(List<Promotion>), response.Headers);
        }

        /// <summary>
        /// Update a promotion. Update a promotion for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="promotionId"></param> 
        /// <param name="promotion"></param> 
        /// <returns></returns>            
        public async Task UpdatePromotion(int? promotionId, Promotion promotion)
        {

            // verify the required parameter 'promotionId' is set
            if (promotionId == null) throw new ApiException(400, "Missing required parameter 'promotionId' when calling UpdatePromotion");

            // verify the required parameter 'promotion' is set
            if (promotion == null) throw new ApiException(400, "Missing required parameter 'promotion' when calling UpdatePromotion");


            var path = "/promotions/{promotionId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "promotionId" + "}", ApiClient.ParameterToString(promotionId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(promotion); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdatePromotion: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdatePromotion: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

    }
}
