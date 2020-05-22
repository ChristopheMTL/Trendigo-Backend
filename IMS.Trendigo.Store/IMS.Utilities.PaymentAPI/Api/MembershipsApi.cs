using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Utilities.PaymentAPI.Client;
using IMS.Utilities.PaymentAPI.Model;
using RestSharp;

namespace IMS.Utilities.PaymentAPI.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IMembershipsApi
    {
        /// <summary>
        /// Add new points for a specific member for a specific membership. Add new point for a specific member for a specific membership.
        /// </summary>
        /// <param name="membershipId"></param>
        /// <param name="point"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> AddMemberPoints(int? membershipId, Point point);
        /// <summary>
        /// Create a new membership for a specific member. Create a new membership for a specific member.
        /// </summary>
        /// <param name="membership"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateMembership(Membership membership);
        /// <summary>
        /// Delete a membership for a specific member. Delete a membership for a specific member.
        /// </summary>
        /// <param name="membershipId"></param>
        /// <returns></returns>
        Task DeleteMembership(int? membershipId);
        /// <summary>
        /// Returns the current number of points of a specific member for a specific membership. Returns the current number of points of a specific member for a specific membership.
        /// </summary>
        /// <param name="membershipId"></param>
        /// <returns>Point</returns>
        Task<Point> FindMemberPoints(int? membershipId);
        /// <summary>
        /// Find a specific membership. Find a specific membership.
        /// </summary>
        /// <param name="membershipId"></param>
        /// <returns>Membership</returns>
        Task<Membership> FindMembership(int? membershipId);
        /// <summary>
        /// Find the list of memberships. Find the list of memberships.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param>
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param>
        /// <param name="maxResults">The number of results to return. Leave empty if you do not want to fix a limit (the maximum is set to 999).</param>
        /// <returns>List&lt;Membership&gt;</returns>
        Task<List<Membership>> FindMemberships(string filter, long? startIndex, int? maxResults);
        /// <summary>
        /// Add new points for a specific member for a specific membership. Add new point for a specific member for a specific membership.
        /// </summary>
        /// <param name="membershipId"></param>
        /// <param name="point"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> SubtractMemberPoints(int? membershipId, Point point);
        /// <summary>
        /// Transfer existing points from a member to another member. Transfer existing points from a member to another member. An error will happen if the criteria for the transfer are invalid (ie: the number of points being transferred exceed the current balance of point).
        /// </summary>
        /// <param name="membershipId"></param>
        /// <param name="targetedMembershipId"></param>
        /// <param name="point">The number of points to be transfered to the targeted membership. This number of point transfered must not exceed the current balance of point in the membership.</param>
        /// <returns>List&lt;EntityId&gt;</returns>
        Task<List<EntityId>> TransferMemberPoints(int? membershipId, int? targetedMembershipId, Point point);
        /// <summary>
        /// Update a Membership. Update a Membership.
        /// </summary>
        /// <param name="membershipId"></param>
        /// <param name="membership"></param>
        /// <returns></returns>
        Task UpdateMembership(int? membershipId, Membership membership);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class MembershipsApi : IMembershipsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public MembershipsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MembershipsApi(String basePath)
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
        /// Add new points for a specific member for a specific membership. Add new point for a specific member for a specific membership.
        /// </summary>
        /// <param name="membershipId"></param> 
        /// <param name="point"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> AddMemberPoints(int? membershipId, Point point)
        {

            // verify the required parameter 'membershipId' is set
            if (membershipId == null) throw new ApiException(400, "Missing required parameter 'membershipId' when calling AddMemberPoints");

            // verify the required parameter 'point' is set
            if (point == null) throw new ApiException(400, "Missing required parameter 'point' when calling AddMemberPoints");


            var path = "/memberships/{membershipId}/points/add";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "membershipId" + "}", ApiClient.ParameterToString(membershipId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(point); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling AddMemberPoints: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling AddMemberPoints: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Create a new membership for a specific member. Create a new membership for a specific member.
        /// </summary>
        /// <param name="membership"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateMembership(Membership membership)
        {

            // verify the required parameter 'membership' is set
            if (membership == null) throw new ApiException(400, "Missing required parameter 'membership' when calling CreateMembership");


            var path = "/memberships/";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(membership); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateMembership: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateMembership: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Delete a membership for a specific member. Delete a membership for a specific member.
        /// </summary>
        /// <param name="membershipId"></param> 
        /// <returns></returns>            
        public async Task DeleteMembership(int? membershipId)
        {

            // verify the required parameter 'membershipId' is set
            if (membershipId == null) throw new ApiException(400, "Missing required parameter 'membershipId' when calling DeleteMembership");


            var path = "/memberships/{membershipId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "membershipId" + "}", ApiClient.ParameterToString(membershipId));

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
                throw new ApiException((int)response.StatusCode, "Error calling DeleteMembership: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteMembership: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Returns the current number of points of a specific member for a specific membership. Returns the current number of points of a specific member for a specific membership.
        /// </summary>
        /// <param name="membershipId"></param> 
        /// <returns>Point</returns>            
        public async Task<Point> FindMemberPoints(int? membershipId)
        {

            // verify the required parameter 'membershipId' is set
            if (membershipId == null) throw new ApiException(400, "Missing required parameter 'membershipId' when calling FindMemberPoints");


            var path = "/memberships/{membershipId}/points";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "membershipId" + "}", ApiClient.ParameterToString(membershipId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindMemberPoints: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindMemberPoints: " + response.ErrorMessage, response.ErrorMessage);

            return (Point)ApiClient.Deserialize(response.Content, typeof(Point), response.Headers);
        }

        /// <summary>
        /// Find a specific membership. Find a specific membership.
        /// </summary>
        /// <param name="membershipId"></param> 
        /// <returns>Membership</returns>            
        public async Task<Membership> FindMembership(int? membershipId)
        {

            // verify the required parameter 'membershipId' is set
            if (membershipId == null) throw new ApiException(400, "Missing required parameter 'membershipId' when calling FindMembership");


            var path = "/memberships/{membershipId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "membershipId" + "}", ApiClient.ParameterToString(membershipId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindMembership: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindMembership: " + response.ErrorMessage, response.ErrorMessage);

            return (Membership)ApiClient.Deserialize(response.Content, typeof(Membership), response.Headers);
        }

        /// <summary>
        /// Find the list of memberships. Find the list of memberships.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param> 
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param> 
        /// <param name="maxResults">The number of results to return. Leave empty if you do not want to fix a limit (the maximum is set to 999).</param> 
        /// <returns>List&lt;Membership&gt;</returns>            
        public async Task<List<Membership>> FindMemberships(string filter, long? startIndex, int? maxResults)
        {


            var path = "/memberships/";
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
                throw new ApiException((int)response.StatusCode, "Error calling FindMemberships: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindMemberships: " + response.ErrorMessage, response.ErrorMessage);

            return (List<Membership>)ApiClient.Deserialize(response.Content, typeof(List<Membership>), response.Headers);
        }

        /// <summary>
        /// Add new points for a specific member for a specific membership. Add new point for a specific member for a specific membership.
        /// </summary>
        /// <param name="membershipId"></param> 
        /// <param name="point"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> SubtractMemberPoints(int? membershipId, Point point)
        {

            // verify the required parameter 'membershipId' is set
            if (membershipId == null) throw new ApiException(400, "Missing required parameter 'membershipId' when calling SubtractMemberPoints");

            // verify the required parameter 'point' is set
            if (point == null) throw new ApiException(400, "Missing required parameter 'point' when calling SubtractMemberPoints");


            var path = "/memberships/{membershipId}/points/subtract";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "membershipId" + "}", ApiClient.ParameterToString(membershipId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(point); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling SubtractMemberPoints: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling SubtractMemberPoints: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Transfer existing points from a member to another member. Transfer existing points from a member to another member. An error will happen if the criteria for the transfer are invalid (ie: the number of points being transferred exceed the current balance of point).
        /// </summary>
        /// <param name="membershipId"></param> 
        /// <param name="targetedMembershipId"></param> 
        /// <param name="point">The number of points to be transfered to the targeted membership. This number of point transfered must not exceed the current balance of point in the membership.</param> 
        /// <returns>List&lt;EntityId&gt;</returns>            
        public async Task<List<EntityId>> TransferMemberPoints(int? membershipId, int? targetedMembershipId, Point point)
        {

            // verify the required parameter 'membershipId' is set
            if (membershipId == null) throw new ApiException(400, "Missing required parameter 'membershipId' when calling TransferMemberPoints");

            // verify the required parameter 'targetedMembershipId' is set
            if (targetedMembershipId == null) throw new ApiException(400, "Missing required parameter 'targetedMembershipId' when calling TransferMemberPoints");

            // verify the required parameter 'point' is set
            if (point == null) throw new ApiException(400, "Missing required parameter 'point' when calling TransferMemberPoints");


            var path = "/memberships/{membershipId}/points/transfer/{targetedMembershipId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "membershipId" + "}", ApiClient.ParameterToString(membershipId));
            path = path.Replace("{" + "targetedMembershipId" + "}", ApiClient.ParameterToString(targetedMembershipId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(point); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling TransferMemberPoints: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling TransferMemberPoints: " + response.ErrorMessage, response.ErrorMessage);

            return (List<EntityId>)ApiClient.Deserialize(response.Content, typeof(List<EntityId>), response.Headers);
        }

        /// <summary>
        /// Update a Membership. Update a Membership.
        /// </summary>
        /// <param name="membershipId"></param> 
        /// <param name="membership"></param> 
        /// <returns></returns>            
        public async Task UpdateMembership(int? membershipId, Membership membership)
        {

            // verify the required parameter 'membershipId' is set
            if (membershipId == null) throw new ApiException(400, "Missing required parameter 'membershipId' when calling UpdateMembership");

            // verify the required parameter 'membership' is set
            if (membership == null) throw new ApiException(400, "Missing required parameter 'membership' when calling UpdateMembership");


            var path = "/memberships/{membershipId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "membershipId" + "}", ApiClient.ParameterToString(membershipId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(membership); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateMembership: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateMembership: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

    }
}
