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
    public interface ITerminalsApi
    {
        /// <summary>
        /// Create a new terminal for a specific merchant. Create a new terminal for a specific merchant.
        /// </summary>
        /// <param name="terminal"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateTerminal(Terminal terminal);
        /// <summary>
        /// Delete a specific terminal. Delete a terminal for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        Task DeleteTerminal(int? terminalId);
        /// <summary>
        /// Find a specific terminal. Find a terminal for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns>Terminal</returns>
        Task<Terminal> FindTerminal(int? terminalId);
        /// <summary>
        /// Find the terminals of a specific merchant. Find all the terminals of a specific merchant with a given merchant identifier.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param>
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param>
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param>
        /// <returns>List&lt;Terminal&gt;</returns>
        Task<List<Terminal>> FindTerminals(string filter, long? startIndex, int? maxResults);
        /// <summary>
        /// Update a specific terminal. Update a terminal for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="terminalId"></param>
        /// <param name="terminal"></param>
        /// <returns></returns>
        Task UpdateTerminal(int? terminalId, Terminal terminal);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class TerminalsApi : ITerminalsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public TerminalsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public TerminalsApi(String basePath)
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
        /// Create a new terminal for a specific merchant. Create a new terminal for a specific merchant.
        /// </summary>
        /// <param name="terminal"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateTerminal(Terminal terminal)
        {

            // verify the required parameter 'terminal' is set
            if (terminal == null) throw new ApiException(400, "Missing required parameter 'terminal' when calling CreateTerminal");


            var path = "/terminals";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(terminal); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateTerminal: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateTerminal: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Delete a specific terminal. Delete a terminal for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="terminalId"></param> 
        /// <returns></returns>            
        public async Task DeleteTerminal(int? terminalId)
        {

            // verify the required parameter 'terminalId' is set
            if (terminalId == null) throw new ApiException(400, "Missing required parameter 'terminalId' when calling DeleteTerminal");


            var path = "/terminals/{terminalId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "terminalId" + "}", ApiClient.ParameterToString(terminalId));

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
                throw new ApiException((int)response.StatusCode, "Error calling DeleteTerminal: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteTerminal: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Find a specific terminal. Find a terminal for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="terminalId"></param> 
        /// <returns>Terminal</returns>            
        public async Task<Terminal> FindTerminal(int? terminalId)
        {

            // verify the required parameter 'terminalId' is set
            if (terminalId == null) throw new ApiException(400, "Missing required parameter 'terminalId' when calling FindTerminal");


            var path = "/terminals/{terminalId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "terminalId" + "}", ApiClient.ParameterToString(terminalId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindTerminal: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindTerminal: " + response.ErrorMessage, response.ErrorMessage);

            return (Terminal)ApiClient.Deserialize(response.Content, typeof(Terminal), response.Headers);
        }

        /// <summary>
        /// Find the terminals of a specific merchant. Find all the terminals of a specific merchant with a given merchant identifier.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param> 
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param> 
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param> 
        /// <returns>List&lt;Terminal&gt;</returns>            
        public async Task<List<Terminal>> FindTerminals(string filter, long? startIndex, int? maxResults)
        {


            var path = "/terminals";
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
                throw new ApiException((int)response.StatusCode, "Error calling FindTerminals: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindTerminals: " + response.ErrorMessage, response.ErrorMessage);

            return (List<Terminal>)ApiClient.Deserialize(response.Content, typeof(List<Terminal>), response.Headers);
        }

        /// <summary>
        /// Update a specific terminal. Update a terminal for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="terminalId"></param> 
        /// <param name="terminal"></param> 
        /// <returns></returns>            
        public async Task UpdateTerminal(int? terminalId, Terminal terminal)
        {

            // verify the required parameter 'terminalId' is set
            if (terminalId == null) throw new ApiException(400, "Missing required parameter 'terminalId' when calling UpdateTerminal");

            // verify the required parameter 'terminal' is set
            if (terminal == null) throw new ApiException(400, "Missing required parameter 'terminal' when calling UpdateTerminal");


            var path = "/terminals/{terminalId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "terminalId" + "}", ApiClient.ParameterToString(terminalId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(terminal); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateTerminal: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateTerminal: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

    }
}
