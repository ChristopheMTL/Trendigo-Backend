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
    public interface IUsersApi
    {
        /// <summary>
        /// Associate a new notification profile for a specific user. Associate a new notification profile for a specific user (a user correspond to the merchant administrative account).
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notification"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> AssociateNotificationToUser(int? userId, Notification notification);
        /// <summary>
        /// Create a new user for a specific merchant. Create a new user for a specific merchant.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateUser(User user);
        /// <summary>
        /// Delete a specific user. Delete a user for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteUser(int? userId);
        /// <summary>
        /// Find a specific user. Find a user for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>User</returns>
        Task<User> FindUser(int? userId);
        /// <summary>
        /// Find the users. Find all the users of a specific merchant with a given merchant identifier.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param>
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param>
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param>
        /// <returns>List&lt;User&gt;</returns>
        Task<List<User>> FindUsers(string filter, long? startIndex, int? maxResults);
        /// <summary>
        /// Update a specific user. Update a user for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUser(int? userId, User user);
        /// <summary>
        /// Announce a new successful login attempt for the specific user. This operation will invalid the precedent token and announce to the Admin API that a new Session Token has been issued for this user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authenticationData"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> UserLogin(int? userId, AuthenticationData authenticationData);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class UsersApi : IUsersApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public UsersApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersApi"/> class.
        /// </summary>
        /// <returns></returns>
        public UsersApi(String basePath)
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
        /// Associate a new notification profile for a specific user. Associate a new notification profile for a specific user (a user correspond to the merchant administrative account).
        /// </summary>
        /// <param name="userId"></param> 
        /// <param name="notification"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> AssociateNotificationToUser(int? userId, Notification notification)
        {

            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling AssociateNotificationToUser");

            // verify the required parameter 'notification' is set
            if (notification == null) throw new ApiException(400, "Missing required parameter 'notification' when calling AssociateNotificationToUser");


            var path = "/users/{userId}/notifications/";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(notification); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling AssociateNotificationToUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling AssociateNotificationToUser: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Create a new user for a specific merchant. Create a new user for a specific merchant.
        /// </summary>
        /// <param name="user"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateUser(User user)
        {

            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling CreateUser");


            var path = "/users";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(user); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateUser: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Delete a specific user. Delete a user for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns></returns>            
        public async Task DeleteUser(int? userId)
        {

            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling DeleteUser");


            var path = "/users/{userId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));

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
                throw new ApiException((int)response.StatusCode, "Error calling DeleteUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteUser: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Find a specific user. Find a user for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns>User</returns>            
        public async Task<User> FindUser(int? userId)
        {

            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling FindUser");


            var path = "/users/{userId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindUser: " + response.ErrorMessage, response.ErrorMessage);

            return (User)ApiClient.Deserialize(response.Content, typeof(User), response.Headers);
        }

        /// <summary>
        /// Find the users. Find all the users of a specific merchant with a given merchant identifier.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param> 
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param> 
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param> 
        /// <returns>List&lt;User&gt;</returns>            
        public async Task<List<User>> FindUsers(string filter, long? startIndex, int? maxResults)
        {


            var path = "/users";
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
                throw new ApiException((int)response.StatusCode, "Error calling FindUsers: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindUsers: " + response.ErrorMessage, response.ErrorMessage);

            return (List<User>)ApiClient.Deserialize(response.Content, typeof(List<User>), response.Headers);
        }

        /// <summary>
        /// Update a specific user. Update a user for a specific merchant with a given identifier.
        /// </summary>
        /// <param name="userId"></param> 
        /// <param name="user"></param> 
        /// <returns></returns>            
        public async Task UpdateUser(int? userId, User user)
        {

            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling UpdateUser");

            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling UpdateUser");


            var path = "/users/{userId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(user); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateUser: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Announce a new successful login attempt for the specific user. This operation will invalid the precedent token and announce to the Admin API that a new Session Token has been issued for this user.
        /// </summary>
        /// <param name="userId"></param> 
        /// <param name="authenticationData"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> UserLogin(int? userId, AuthenticationData authenticationData)
        {

            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling UserLogin");

            // verify the required parameter 'authenticationData' is set
            if (authenticationData == null) throw new ApiException(400, "Missing required parameter 'authenticationData' when calling UserLogin");


            var path = "/users/{userId}/login/";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(authenticationData); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UserLogin: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UserLogin: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

    }
}
