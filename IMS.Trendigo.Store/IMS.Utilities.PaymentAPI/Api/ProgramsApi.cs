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
    public interface IProgramsApi
    {
        /// <summary>
        /// Create a new program on the system. Create a new program on the system.
        /// </summary>
        /// <param name="program"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateProgram(Program program);
        /// <summary>
        /// Delete a Program on the system. Delete a Program on the system.
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        Task DeleteProgram(int? programId);
        /// <summary>
        /// Find a specific Program. Find a specific Program.
        /// </summary>
        /// <param name="programId"></param>
        /// <returns>Program</returns>
        Task<Program> FindProgram(int? programId);
        /// <summary>
        /// Find the list of programs based on the search criteria. Find the list of programs across the system based on the search criteria.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param>
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param>
        /// <param name="maxResults">The number of results to return. Leave empty if you do not want to fix a limit (the maximum is set to 999).</param>
        /// <returns>List&lt;Program&gt;</returns>
        Task<List<Program>> FindPrograms(string filter, long? startIndex, int? maxResults);
        /// <summary>
        /// Update a program. Update a program.
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="program"></param>
        /// <returns></returns>
        Task UpdateProgram(int? programId, Program program);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ProgramsApi : IProgramsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ProgramsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ProgramsApi(String basePath)
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
        /// Create a new program on the system. Create a new program on the system.
        /// </summary>
        /// <param name="program"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateProgram(Program program)
        {

            // verify the required parameter 'program' is set
            if (program == null) throw new ApiException(400, "Missing required parameter 'program' when calling CreateProgram");


            var path = "/programs";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(program); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateProgram: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateProgram: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Delete a Program on the system. Delete a Program on the system.
        /// </summary>
        /// <param name="programId"></param> 
        /// <returns></returns>            
        public async Task DeleteProgram(int? programId)
        {

            // verify the required parameter 'programId' is set
            if (programId == null) throw new ApiException(400, "Missing required parameter 'programId' when calling DeleteProgram");


            var path = "/programs/{programId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "programId" + "}", ApiClient.ParameterToString(programId));

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
                throw new ApiException((int)response.StatusCode, "Error calling DeleteProgram: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteProgram: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Find a specific Program. Find a specific Program.
        /// </summary>
        /// <param name="programId"></param> 
        /// <returns>Program</returns>            
        public async Task<Program> FindProgram(int? programId)
        {

            // verify the required parameter 'programId' is set
            if (programId == null) throw new ApiException(400, "Missing required parameter 'programId' when calling FindProgram");


            var path = "/programs/{programId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "programId" + "}", ApiClient.ParameterToString(programId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindProgram: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindProgram: " + response.ErrorMessage, response.ErrorMessage);

            return (Program)ApiClient.Deserialize(response.Content, typeof(Program), response.Headers);
        }

        /// <summary>
        /// Find the list of programs based on the search criteria. Find the list of programs across the system based on the search criteria.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param> 
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param> 
        /// <param name="maxResults">The number of results to return. Leave empty if you do not want to fix a limit (the maximum is set to 999).</param> 
        /// <returns>List&lt;Program&gt;</returns>            
        public async Task<List<Program>> FindPrograms(string filter, long? startIndex, int? maxResults)
        {


            var path = "/programs";
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
                throw new ApiException((int)response.StatusCode, "Error calling FindPrograms: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindPrograms: " + response.ErrorMessage, response.ErrorMessage);

            return (List<Program>)ApiClient.Deserialize(response.Content, typeof(List<Program>), response.Headers);
        }

        /// <summary>
        /// Update a program. Update a program.
        /// </summary>
        /// <param name="programId"></param> 
        /// <param name="program"></param> 
        /// <returns></returns>            
        public async Task UpdateProgram(int? programId, Program program)
        {

            // verify the required parameter 'programId' is set
            if (programId == null) throw new ApiException(400, "Missing required parameter 'programId' when calling UpdateProgram");

            // verify the required parameter 'program' is set
            if (program == null) throw new ApiException(400, "Missing required parameter 'program' when calling UpdateProgram");


            var path = "/programs/{programId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "programId" + "}", ApiClient.ParameterToString(programId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(program); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateProgram: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateProgram: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

    }
}
