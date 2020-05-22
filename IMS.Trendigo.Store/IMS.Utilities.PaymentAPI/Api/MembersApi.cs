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
    public interface IMembersApi
    {
        /// <summary>
        /// Associate a new notification profile for a specific member. Associate a new notification profile for a specific member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="notification"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> AssociateNotificationToMember(int? memberId, Notification notification);
        /// <summary>
        /// Create a new Trendigo card for a specific member. Create a new Trendigo card for a specific member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="card"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateCard(int? memberId, Card card);
        /// <summary>
        /// Create a new credit card for a specific member. Create a new credit card for a specific member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="creditcard"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateCreditCard(int? memberId, Creditcard creditcard);
        /// <summary>
        /// Create a new member. Create a new member.
        /// </summary>
        /// <param name="member"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> CreateMember(Member member);
        /// <summary>
        /// Delete a member Trendigo Card. Delete a member Trendigo Card for a member..
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        Task DeleteCard(int? memberId, int? cardId);
        /// <summary>
        /// Delete a credit card for a specific member. Delete a credit card for a specific member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="creditCardId"></param>
        /// <returns></returns>
        Task DeleteCreditCard(int? memberId, int? creditCardId);
        /// <summary>
        /// Delete a specific member.&#39; Delete a member with a given identifier.
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        Task DeleteMember(int? memberId);
        /// <summary>
        /// Find a Trendigo card of a specific member. Find a Trendigo card of a specific member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="cardId"></param>
        /// <returns>Card</returns>
        Task<Card> FindCard(int? memberId, int? cardId);
        /// <summary>
        /// Find the list of Trendigo cards for a specific member. Find the list of Trendigo cards for a specific member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>List&lt;Card&gt;</returns>
        Task<List<Card>> FindCards(int? memberId);
        /// <summary>
        /// Find a credit cards of a specific member. Find a credit cards of a specific member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="creditCardId"></param>
        /// <returns>Creditcard</returns>
        Task<Creditcard> FindCreditCard(int? memberId, int? creditCardId);
        /// <summary>
        /// Find the list of credit cards of a specific member. Find the list of credit cards of a specific member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>List&lt;Creditcard&gt;</returns>
        Task<List<Creditcard>> FindCreditCards(int? memberId);
        /// <summary>
        /// Find a specific member. Find a member with a given identifier.
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>Member</returns>
        Task<Member> FindMember(int? memberId);
        /// <summary>
        /// Return the list of memberships for a specific member. Return the list of memberships for a specific member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>List&lt;Membership&gt;</returns>
        Task<List<Membership>> FindMemberMemberships(int? memberId);
        /// <summary>
        /// Search specific members matching a given pattern. Searches for members (memberName, displayName, email) that match a certain pattern. Use a wildcard character (&#39;*&#39;) to match any number of characters.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param>
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param>
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param>
        /// <returns>List&lt;Member&gt;</returns>
        Task<List<Member>> FindMembers(string filter, long? startIndex, int? maxResults);
        /// <summary>
        /// Announce a new successful login attempt for the specific member. This operation will invalid the precedent Session Token and announce to the Admin API that a new Session Token has been issued for this Member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="authenticationData"></param>
        /// <returns>EntityId</returns>
        Task<EntityId> MemberLogin(int? memberId, AuthenticationData authenticationData);
        /// <summary>
        /// Release an already assigned Trendigo Card. Release an already assigned Trendigo Card.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        Task ReleaseCard(int? memberId, int? cardId);
        /// <summary>
        /// Identify as the default credit card. Identify a credit card as the default credit card for a member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="creditCardId"></param>
        /// <returns></returns>
        Task SetDefaultCreditCard(int? memberId, int? creditCardId);
        /// <summary>
        /// Update a member Trendigo Card. Update a member Trendigo card for a member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="cardId"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        Task UpdateCard(int? memberId, int? cardId, Card card);
        /// <summary>
        /// Update a member credit card. Update a member credit card for a member.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="creditCardId"></param>
        /// <param name="creditcard"></param>
        /// <returns></returns>
        Task UpdateCreditCard(int? memberId, int? creditCardId, Creditcard creditcard);
        /// <summary>
        /// Update a specific member. Update a member given a member identifier.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task UpdateMember(int? memberId, Member member);
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class MembersApi : IMembersApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MembersApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public MembersApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MembersApi(String basePath)
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
        /// Associate a new notification profile for a specific member. Associate a new notification profile for a specific member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="notification"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> AssociateNotificationToMember(int? memberId, Notification notification)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling AssociateNotificationToMember");

            // verify the required parameter 'notification' is set
            if (notification == null) throw new ApiException(400, "Missing required parameter 'notification' when calling AssociateNotificationToMember");


            var path = "/members/{memberId}/notifications/";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));

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
                throw new ApiException((int)response.StatusCode, "Error calling AssociateNotificationToMember: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling AssociateNotificationToMember: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Create a new Trendigo card for a specific member. Create a new Trendigo card for a specific member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="card"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateCard(int? memberId, Card card)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling CreateCard");

            // verify the required parameter 'card' is set
            if (card == null) throw new ApiException(400, "Missing required parameter 'card' when calling CreateCard");


            var path = "/members/{memberId}/cards/";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(card); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateCard: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateCard: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Create a new credit card for a specific member. Create a new credit card for a specific member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="creditcard"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateCreditCard(int? memberId, Creditcard creditcard)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling CreateCreditCard");

            // verify the required parameter 'creditcard' is set
            if (creditcard == null) throw new ApiException(400, "Missing required parameter 'creditcard' when calling CreateCreditCard");


            var path = "/members/{memberId}/creditcards/";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(creditcard); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateCreditCard: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateCreditCard: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Create a new member. Create a new member.
        /// </summary>
        /// <param name="member"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> CreateMember(Member member)
        {

            // verify the required parameter 'member' is set
            if (member == null) throw new ApiException(400, "Missing required parameter 'member' when calling CreateMember");


            var path = "/members/";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(member); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling CreateMember: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling CreateMember: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Delete a member Trendigo Card. Delete a member Trendigo Card for a member..
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="cardId"></param> 
        /// <returns></returns>            
        public async Task DeleteCard(int? memberId, int? cardId)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling DeleteCard");

            // verify the required parameter 'cardId' is set
            if (cardId == null) throw new ApiException(400, "Missing required parameter 'cardId' when calling DeleteCard");


            var path = "/members/{memberId}/cards/{cardId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));
            path = path.Replace("{" + "cardId" + "}", ApiClient.ParameterToString(cardId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;


            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteCard: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteCard: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Delete a credit card for a specific member. Delete a credit card for a specific member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="creditCardId"></param> 
        /// <returns></returns>            
        public async Task DeleteCreditCard(int? memberId, int? creditCardId)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling DeleteCreditCard");

            // verify the required parameter 'creditCardId' is set
            if (creditCardId == null) throw new ApiException(400, "Missing required parameter 'creditCardId' when calling DeleteCreditCard");


            var path = "/members/{memberId}/creditcards/{creditCardId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));
            path = path.Replace("{" + "creditCardId" + "}", ApiClient.ParameterToString(creditCardId));

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
                throw new ApiException((int)response.StatusCode, "Error calling DeleteCreditCard: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteCreditCard: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Delete a specific member.&#39; Delete a member with a given identifier.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <returns></returns>            
        public async Task DeleteMember(int? memberId)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling DeleteMember");


            var path = "/members/{memberId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));

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
                throw new ApiException((int)response.StatusCode, "Error calling DeleteMember: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling DeleteMember: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Find a Trendigo card of a specific member. Find a Trendigo card of a specific member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="cardId"></param> 
        /// <returns>Card</returns>            
        public async Task<Card> FindCard(int? memberId, int? cardId)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling FindCard");

            // verify the required parameter 'cardId' is set
            if (cardId == null) throw new ApiException(400, "Missing required parameter 'cardId' when calling FindCard");


            var path = "/members/{memberId}/cards/{cardId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));
            path = path.Replace("{" + "cardId" + "}", ApiClient.ParameterToString(cardId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindCard: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindCard: " + response.ErrorMessage, response.ErrorMessage);

            return (Card)ApiClient.Deserialize(response.Content, typeof(Card), response.Headers);
        }

        /// <summary>
        /// Find the list of Trendigo cards for a specific member. Find the list of Trendigo cards for a specific member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <returns>List&lt;Card&gt;</returns>            
        public async Task<List<Card>> FindCards(int? memberId)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling FindCards");


            var path = "/members/{memberId}/cards/";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindCards: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindCards: " + response.ErrorMessage, response.ErrorMessage);

            return (List<Card>)ApiClient.Deserialize(response.Content, typeof(List<Card>), response.Headers);
        }

        /// <summary>
        /// Find a credit cards of a specific member. Find a credit cards of a specific member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="creditCardId"></param> 
        /// <returns>Creditcard</returns>            
        public async Task<Creditcard> FindCreditCard(int? memberId, int? creditCardId)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling FindCreditCard");

            // verify the required parameter 'creditCardId' is set
            if (creditCardId == null) throw new ApiException(400, "Missing required parameter 'creditCardId' when calling FindCreditCard");


            var path = "/members/{memberId}/creditcards/{creditCardId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));
            path = path.Replace("{" + "creditCardId" + "}", ApiClient.ParameterToString(creditCardId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindCreditCard: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindCreditCard: " + response.ErrorMessage, response.ErrorMessage);

            return (Creditcard)ApiClient.Deserialize(response.Content, typeof(Creditcard), response.Headers);
        }

        /// <summary>
        /// Find the list of credit cards of a specific member. Find the list of credit cards of a specific member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <returns>List&lt;Creditcard&gt;</returns>            
        public async Task<List<Creditcard>> FindCreditCards(int? memberId)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling FindCreditCards");


            var path = "/members/{memberId}/creditcards/";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindCreditCards: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindCreditCards: " + response.ErrorMessage, response.ErrorMessage);

            return (List<Creditcard>)ApiClient.Deserialize(response.Content, typeof(List<Creditcard>), response.Headers);
        }

        /// <summary>
        /// Find a specific member. Find a member with a given identifier.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <returns>Member</returns>            
        public async Task<Member> FindMember(int? memberId)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling FindMember");


            var path = "/members/{memberId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindMember: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindMember: " + response.ErrorMessage, response.ErrorMessage);

            return (Member)ApiClient.Deserialize(response.Content, typeof(Member), response.Headers);
        }

        /// <summary>
        /// Return the list of memberships for a specific member. Return the list of memberships for a specific member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <returns>List&lt;Membership&gt;</returns>            
        public async Task<List<Membership>> FindMemberMemberships(int? memberId)
        {
            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling FindMemberMemberships");


            var path = "/members/{memberId}/memberships";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));

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
                throw new ApiException((int)response.StatusCode, "Error calling FindMemberMemberships: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindMemberMemberships: " + response.ErrorMessage, response.ErrorMessage);

            return (List<Membership>)ApiClient.Deserialize(response.Content, typeof(List<Membership>), response.Headers);
        }

        /// <summary>
        /// Search specific members matching a given pattern. Searches for members (memberName, displayName, email) that match a certain pattern. Use a wildcard character (&#39;*&#39;) to match any number of characters.
        /// </summary>
        /// <param name="filter">The search filter, wildcards (&#39;*&#39;) must be used to match any characters. Can be missing or empty to list all.</param> 
        /// <param name="startIndex">The index of the record to start returning. 0 to start at the first record.</param> 
        /// <param name="maxResults">The number of results to return. -1 to return all results (might consume a lot of memory if the list is large)</param> 
        /// <returns>List&lt;Member&gt;</returns>            
        public async Task<List<Member>> FindMembers(string filter, long? startIndex, int? maxResults)
        {
            var path = "/members/";
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
                throw new ApiException((int)response.StatusCode, "Error calling FindMembers: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling FindMembers: " + response.ErrorMessage, response.ErrorMessage);

            return (List<Member>)ApiClient.Deserialize(response.Content, typeof(List<Member>), response.Headers);
        }

        /// <summary>
        /// Announce a new successful login attempt for the specific member. This operation will invalid the precedent Session Token and announce to the Admin API that a new Session Token has been issued for this Member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="authenticationData"></param> 
        /// <returns>EntityId</returns>            
        public async Task<EntityId> MemberLogin(int? memberId, AuthenticationData authenticationData)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling MemberLogin");

            // verify the required parameter 'authenticationData' is set
            if (authenticationData == null) throw new ApiException(400, "Missing required parameter 'authenticationData' when calling MemberLogin");


            var path = "/members/{memberId}/login";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));

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
                throw new ApiException((int)response.StatusCode, "Error calling MemberLogin: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling MemberLogin: " + response.ErrorMessage, response.ErrorMessage);

            return (EntityId)ApiClient.Deserialize(response.Content, typeof(EntityId), response.Headers);
        }

        /// <summary>
        /// Release an already assigned Trendigo Card. Release an already assigned Trendigo Card.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="cardId"></param> 
        /// <returns></returns>            
        public async Task ReleaseCard(int? memberId, int? cardId)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling ReleaseCard");

            // verify the required parameter 'cardId' is set
            if (cardId == null) throw new ApiException(400, "Missing required parameter 'cardId' when calling ReleaseCard");


            var path = "/members/{memberId}/cards/{cardId}/release";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));
            path = path.Replace("{" + "cardId" + "}", ApiClient.ParameterToString(cardId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;


            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling ReleaseCard: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling ReleaseCard: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Identify as the default credit card. Identify a credit card as the default credit card for a member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="creditCardId"></param> 
        /// <returns></returns>            
        public async Task SetDefaultCreditCard(int? memberId, int? creditCardId)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling SetDefaultCreditCard");

            // verify the required parameter 'creditCardId' is set
            if (creditCardId == null) throw new ApiException(400, "Missing required parameter 'creditCardId' when calling SetDefaultCreditCard");


            var path = "/members/{memberId}/creditcards/{creditCardId}/setDefaultCard";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));
            path = path.Replace("{" + "creditCardId" + "}", ApiClient.ParameterToString(creditCardId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;


            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling SetDefaultCreditCard: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling SetDefaultCreditCard: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Update a member Trendigo Card. Update a member Trendigo card for a member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="cardId"></param> 
        /// <param name="card"></param> 
        /// <returns></returns>            
        public async Task UpdateCard(int? memberId, int? cardId, Card card)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling UpdateCard");

            // verify the required parameter 'cardId' is set
            if (cardId == null) throw new ApiException(400, "Missing required parameter 'cardId' when calling UpdateCard");

            // verify the required parameter 'card' is set
            if (card == null) throw new ApiException(400, "Missing required parameter 'card' when calling UpdateCard");


            var path = "/members/{memberId}/cards/{cardId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));
            path = path.Replace("{" + "cardId" + "}", ApiClient.ParameterToString(cardId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(card); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateCard: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateCard: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Update a member credit card. Update a member credit card for a member.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="creditCardId"></param> 
        /// <param name="creditcard"></param> 
        /// <returns></returns>            
        public async Task UpdateCreditCard(int? memberId, int? creditCardId, Creditcard creditcard)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling UpdateCreditCard");

            // verify the required parameter 'creditCardId' is set
            if (creditCardId == null) throw new ApiException(400, "Missing required parameter 'creditCardId' when calling UpdateCreditCard");

            // verify the required parameter 'creditcard' is set
            if (creditcard == null) throw new ApiException(400, "Missing required parameter 'creditcard' when calling UpdateCreditCard");


            var path = "/members/{memberId}/creditcards/{creditCardId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));
            path = path.Replace("{" + "creditCardId" + "}", ApiClient.ParameterToString(creditCardId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(creditcard); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateCreditCard: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateCreditCard: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

        /// <summary>
        /// Update a specific member. Update a member given a member identifier.
        /// </summary>
        /// <param name="memberId"></param> 
        /// <param name="member"></param> 
        /// <returns></returns>            
        public async Task UpdateMember(int? memberId, Member member)
        {

            // verify the required parameter 'memberId' is set
            if (memberId == null) throw new ApiException(400, "Missing required parameter 'memberId' when calling UpdateMember");

            // verify the required parameter 'member' is set
            if (member == null) throw new ApiException(400, "Missing required parameter 'member' when calling UpdateMember");


            var path = "/members/{memberId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "memberId" + "}", ApiClient.ParameterToString(memberId));

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            postBody = ApiClient.Serialize(member); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)await ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateMember: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling UpdateMember: " + response.ErrorMessage, response.ErrorMessage);

            return;
        }

    }
}
