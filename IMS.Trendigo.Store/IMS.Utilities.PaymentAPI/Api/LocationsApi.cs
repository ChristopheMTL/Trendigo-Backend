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
    public interface ILocationsApi
    {
        /// <summary>
        /// Create a new location for a specific merchant. Create a new location for a specific merchant.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateLocation(Location location);
        /// <summary>
        /// Delete a specific location. Delete a location for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        Task DeleteLocation(int? locationId);
        /// <summary>
        /// Find a specific location. Find a location for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns>Location</returns>
        Task<Location> FindLocation(int? locationId);
        /// <summary>
        /// Update a specific location. Update a location for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        Task UpdateLocation(int? locationId, Location location);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class LocationsApi : ILocationsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public LocationsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public LocationsApi(String basePath)
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
        /// Create a new location for a specific merchant. Create a new location for a specific merchant.
        /// </summary>
        /// <param name="location"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateLocation(Location location)
        {

            // verify the required parameter 'location' is set
            if (location == null) throw new ApiException(400, "Missing required parameter 'location' when calling CreateLocation");


            var path = "/locations";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(location); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateLocation: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Delete a specific location. Delete a location for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="locationId"></param> 
        /// <returns></returns>            
        public async Task DeleteLocation(int? locationId)
        {

            // verify the required parameter 'locationId' is set
            if (locationId == null) throw new ApiException(400, "Missing required parameter 'locationId' when calling DeleteLocation");


            var path = "/locations/{locationId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "locationId" + "}", ApiClient.ParameterToString(locationId));

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
                throw new ApiException((int)response.StatusCode, "Error calling DeleteLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteLocation: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Find a specific location. Find a location for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="locationId"></param> 
        /// <returns>Location</returns>            
        public async Task<Location> FindLocation(int? locationId)
        {

            // verify the required parameter 'locationId' is set
            if (locationId == null) throw new ApiException(400, "Missing required parameter 'locationId' when calling FindLocation");


            var path = "/locations/{locationId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "locationId" + "}", ApiClient.ParameterToString(locationId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindLocation: " + response.ErrorMessage, response.ErrorMessage);

            return (Location)ApiClient.Deserialize(response.Content, typeof(Location), response.Headers);
        }

        /// <summary>
        /// Update a specific location. Update a location for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="locationId"></param> 
        /// <param name="location"></param> 
        /// <returns></returns>            
        public async Task UpdateLocation(int? locationId, Location location)
        {

            // verify the required parameter 'locationId' is set
            if (locationId == null) throw new ApiException(400, "Missing required parameter 'locationId' when calling UpdateLocation");

            // verify the required parameter 'location' is set
            if (location == null) throw new ApiException(400, "Missing required parameter 'location' when calling UpdateLocation");


            var path = "/locations/{locationId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "locationId" + "}", ApiClient.ParameterToString(locationId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(location); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateLocation: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

    }
}
