using IMS.Utilities.PaymentAPI.Client;
using IMS.Utilities.PaymentAPI.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Utilities.PaymentAPI.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IProcessorsApi
    {
        /// <summary>
        /// Find the processors. Find all the processors.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param>
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param>
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param>
        /// <returns>List&lt;Processor&gt;</returns>
        Task<List<Processor>> FindProcessors(string filter, long? startIndex, int? maxResults);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ProcessorsApi : IProcessorsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ProcessorsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ProcessorsApi(String basePath)
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
        /// Find the processors. Find all the processors.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param> 
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param> 
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param> 
        /// <returns>List&lt;Processor&gt;</returns>            
        public async Task<List<Processor>> FindProcessors(string filter, long? startIndex, int? maxResults)
        {


            var path = "/processors";
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
                throw new ApiException((int)response.StatusCode, "Error calling FindProcessors: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindProcessors: " + response.ErrorMessage, response.ErrorMessage);

            return (List<Processor>)ApiClient.Deserialize(response.Content, typeof(List<Processor>), response.Headers);
        }

    }
}
