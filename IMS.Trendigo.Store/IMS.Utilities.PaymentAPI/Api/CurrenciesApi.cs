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
    public interface ICurrenciesApi
    {
        /// <summary>
        /// Add a new Currency Rate. Add a new Currency Rate.
        /// </summary>
        /// <param name="currencyRate"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateCurrencyRate(CurrencyRate currencyRate);
        /// <summary>
        /// Delete a Currency Rate on the system. Delete a Currency Rate on the system.
        /// </summary>
        /// <param name="currencyRateId"></param>
        /// <returns></returns>
        Task DeleteCurrencyRate(int? currencyRateId);
        /// <summary>
        /// Find a specific Currency Rate. Find a specific Currency Rate.
        /// </summary>
        /// <param name="currencyRateId"></param>
        /// <returns>List&lt;CurrencyRate&gt;</returns>
        Task<List<CurrencyRate>> FindCurrencyRate(int? currencyRateId);
        /// <summary>
        /// Update a Currency Rate. Update a Currency Value.
        /// </summary>
        /// <param name="currencyRateId"></param>
        /// <param name="currencyRate"></param>
        /// <returns></returns>
        Task UpdateCurrencyRate(int? currencyRateId, CurrencyRate currencyRate);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CurrenciesApi : ICurrenciesApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrenciesApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public CurrenciesApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrenciesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CurrenciesApi(String basePath)
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
        /// Add a new Currency Rate. Add a new Currency Rate.
        /// </summary>
        /// <param name="currencyRate"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateCurrencyRate(CurrencyRate currencyRate)
        {

            // verify the required parameter 'currencyRate' is set
            if (currencyRate == null) throw new ApiException(400, "Missing required parameter 'currencyRate' when calling CreateCurrencyRate");


            var path = "/currencyrates";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(currencyRate); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateCurrencyRate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateCurrencyRate: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Delete a Currency Rate on the system. Delete a Currency Rate on the system.
        /// </summary>
        /// <param name="currencyRateId"></param> 
        /// <returns></returns>            
        public async Task DeleteCurrencyRate(int? currencyRateId)
        {

            // verify the required parameter 'currencyRateId' is set
            if (currencyRateId == null) throw new ApiException(400, "Missing required parameter 'currencyRateId' when calling DeleteCurrencyRate");


            var path = "/currencyrates/{currencyRateId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "currencyRateId" + "}", ApiClient.ParameterToString(currencyRateId));

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
                throw new ApiException((int)response.StatusCode, "Error calling DeleteCurrencyRate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteCurrencyRate: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Find a specific Currency Rate. Find a specific Currency Rate.
        /// </summary>
        /// <param name="currencyRateId"></param> 
        /// <returns>List&lt;CurrencyRate&gt;</returns>            
        public async Task<List<CurrencyRate>> FindCurrencyRate(int? currencyRateId)
        {

            // verify the required parameter 'currencyRateId' is set
            if (currencyRateId == null) throw new ApiException(400, "Missing required parameter 'currencyRateId' when calling FindCurrencyRate");


            var path = "/currencyrates/{currencyRateId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "currencyRateId" + "}", ApiClient.ParameterToString(currencyRateId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindCurrencyRate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindCurrencyRate: " + response.ErrorMessage, response.ErrorMessage);

            return (List<CurrencyRate>)ApiClient.Deserialize(response.Content, typeof(List<CurrencyRate>), response.Headers);
        }

        /// <summary>
        /// Update a Currency Rate. Update a Currency Value.
        /// </summary>
        /// <param name="currencyRateId"></param> 
        /// <param name="currencyRate"></param> 
        /// <returns></returns>            
        public async Task UpdateCurrencyRate(int? currencyRateId, CurrencyRate currencyRate)
        {

            // verify the required parameter 'currencyRateId' is set
            if (currencyRateId == null) throw new ApiException(400, "Missing required parameter 'currencyRateId' when calling UpdateCurrencyRate");

            // verify the required parameter 'currencyRate' is set
            if (currencyRate == null) throw new ApiException(400, "Missing required parameter 'currencyRate' when calling UpdateCurrencyRate");


            var path = "/currencyrates/{currencyRateId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "currencyRateId" + "}", ApiClient.ParameterToString(currencyRateId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(currencyRate); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateCurrencyRate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateCurrencyRate: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

    }
}
