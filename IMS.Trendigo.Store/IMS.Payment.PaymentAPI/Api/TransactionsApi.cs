using System;
using System.Collections.Generic;
using RestSharp;
using IMS.Payment.PaymentAPI.Client;
using IMS.Payment.PaymentAPI.Model;

namespace IMS.Payment.PaymentAPI.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ITransactionsApi
    {
        /// <summary>
        /// Void an existing transaction. Void an existing transaction. The content of the Void Request will be kept in the Transaction Token that has been previously validated.
        /// </summary>
        /// <param name="transactionToken">This is the token obtain after a successful void validation operation. It contains all the information that was submitted in the validation request.</param>
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param>
        /// <returns>VoidResponse</returns>
        VoidResponse CallVoid (string transactionToken, string locale);
        /// <summary>
        /// Make a purchase Make a purchase. The content of the Purchase Request will be kept in the Transaction Token that has been previously validated. The variable parameter that could be filled by the member when a static merchant QR Code is used and the selected Credit Card identifier and passed in the request body.
        /// </summary>
        /// <param name="transactionToken">This is the token obtain after a successful purchase validation operation. It contains all the information that was submitted in the validation request.</param>
        /// <param name="purchaseRequest">The elements in the purchaseRequest are those that are variables and could not have been inserted in the Transaction Token.</param>
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param>
        /// <returns>PurchaseResponse</returns>
        PurchaseResponse Purchase (string transactionToken, PurchaseRequest purchaseRequest, string locale);
        /// <summary>
        /// Perform a purchase validation. Perform a purchase validation. The element present in the request are coming from the merchant QR Code. The result of this operation is the generation of a TransactionToken.
        /// </summary>
        /// <param name="sessionToken">This is the  token obtain after a successful authentication on the Admin API.</param>
        /// <param name="deviceId">This element is what identifies the member in the Payment API. Each member can only have one deviceId active at the same time in the application.</param>
        /// <param name="purchaseValidationRequest"></param>
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param>
        /// <returns>PurchaseValidationResponse</returns>
        PurchaseValidationResponse PurchaseValidation (string sessionToken, string deviceId, PurchaseValidationRequest purchaseValidationRequest, string locale);
        /// <summary>
        /// Refund an authorized transaction. Refund an authorized transaction. The content of the Refund Request will be kept in the Transaction Token that has been previously validated.
        /// </summary>
        /// <param name="transactionToken">This is the token obtain after a successful refund validation operation. It contains all the information that was submitted in the validation request.</param>
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param>
        /// <returns>RefundResponse</returns>
        RefundResponse Refund (string transactionToken, string locale);
        /// <summary>
        /// Perform a refund validation. Perform a refund validation. The element present in the request identifies the purchase transaction that need to be refunded. The result of this operation is the generation of a TransactionToken.
        /// </summary>
        /// <param name="sessionToken">This is the  token obtain after a successful authentication on the Admin API.</param>
        /// <param name="deviceId">This element is what identifies the member in the Payment API. Each member can only have one deviceId active at the same time in the application. But because the refund operation is made by the Trendigo Backend (which is not a mobile device with a deviceId), the special constant TRENDIGO-BACKEND must be used. This will tell to the Payment API that the transaction does not originate from a mobile device.</param>
        /// <param name="refundRequest"></param>
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param>
        /// <returns>RefundValidationResponse</returns>
        RefundValidationResponse RefundValidation (string sessionToken, string deviceId, RefundValidationRequest refundRequest, string locale);
        /// <summary>
        /// Perform a void validation. Perform a void validation. The element present in the request identifies the void transaction that need to be voided. The result of this operation is the generation of a TransactionToken.
        /// </summary>
        /// <param name="sessionToken">This is the token obtain after a successful authentication on the Admin API.</param>
        /// <param name="deviceId">This element is what identifies the clerk in the Payment API. Each clerk can only have one deviceId active at the same time in the application. A validation will be made to ensure that the clerk is voiding a transaction that was made by him.</param>
        /// <param name="voidValidationRequest"></param>
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param>
        /// <returns>VoidValidationResponse</returns>
        VoidValidationResponse VoidValidation (string sessionToken, string deviceId, VoidValidationRequest voidValidationRequest, string locale);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class TransactionsApi : ITransactionsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public TransactionsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public TransactionsApi(String basePath)
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
        public ApiClient ApiClient {get; set;}
    
        /// <summary>
        /// Void an existing transaction. Void an existing transaction. The content of the Void Request will be kept in the Transaction Token that has been previously validated.
        /// </summary>
        /// <param name="transactionToken">This is the token obtain after a successful void validation operation. It contains all the information that was submitted in the validation request.</param> 
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param> 
        /// <returns>VoidResponse</returns>            
        public VoidResponse CallVoid (string transactionToken, string locale)
        {
            
            // verify the required parameter 'transactionToken' is set
            if (transactionToken == null) throw new ApiException(400, "Missing required parameter 'transactionToken' when calling CallVoid");
            
    
            var path = "/payments/void";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                         if (transactionToken != null) headerParams.Add("transactionToken", ApiClient.ParameterToString(transactionToken)); // header parameter
 if (locale != null) headerParams.Add("locale", ApiClient.ParameterToString(locale)); // header parameter
                            
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CallVoid: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CallVoid: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VoidResponse) ApiClient.Deserialize(response.Content, typeof(VoidResponse), response.Headers);
        }
    
        /// <summary>
        /// Make a purchase Make a purchase. The content of the Purchase Request will be kept in the Transaction Token that has been previously validated. The variable parameter that could be filled by the member when a static merchant QR Code is used and the selected Credit Card identifier and passed in the request body.
        /// </summary>
        /// <param name="transactionToken">This is the token obtain after a successful purchase validation operation. It contains all the information that was submitted in the validation request.</param> 
        /// <param name="purchaseRequest">The elements in the purchaseRequest are those that are variables and could not have been inserted in the Transaction Token.</param> 
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param> 
        /// <returns>PurchaseResponse</returns>            
        public PurchaseResponse Purchase (string transactionToken, PurchaseRequest purchaseRequest, string locale)
        {
            
            // verify the required parameter 'transactionToken' is set
            if (transactionToken == null) throw new ApiException(400, "Missing required parameter 'transactionToken' when calling Purchase");
            
            // verify the required parameter 'purchaseRequest' is set
            if (purchaseRequest == null) throw new ApiException(400, "Missing required parameter 'purchaseRequest' when calling Purchase");
            
    
            var path = "/payments/purchase";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                         if (transactionToken != null) headerParams.Add("transactionToken", ApiClient.ParameterToString(transactionToken)); // header parameter
 if (locale != null) headerParams.Add("locale", ApiClient.ParameterToString(locale)); // header parameter
                        postBody = ApiClient.Serialize(purchaseRequest); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling Purchase: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling Purchase: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PurchaseResponse) ApiClient.Deserialize(response.Content, typeof(PurchaseResponse), response.Headers);
        }
    
        /// <summary>
        /// Perform a purchase validation. Perform a purchase validation. The element present in the request are coming from the merchant QR Code. The result of this operation is the generation of a TransactionToken.
        /// </summary>
        /// <param name="sessionToken">This is the  token obtain after a successful authentication on the Admin API.</param> 
        /// <param name="deviceId">This element is what identifies the member in the Payment API. Each member can only have one deviceId active at the same time in the application.</param> 
        /// <param name="purchaseValidationRequest"></param> 
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param> 
        /// <returns>PurchaseValidationResponse</returns>            
        public PurchaseValidationResponse PurchaseValidation (string sessionToken, string deviceId, PurchaseValidationRequest purchaseValidationRequest, string locale)
        {
            
            // verify the required parameter 'sessionToken' is set
            if (sessionToken == null) throw new ApiException(400, "Missing required parameter 'sessionToken' when calling PurchaseValidation");
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling PurchaseValidation");
            
            // verify the required parameter 'purchaseValidationRequest' is set
            if (purchaseValidationRequest == null) throw new ApiException(400, "Missing required parameter 'purchaseValidationRequest' when calling PurchaseValidation");
            
    
            var path = "/payments/purchaseValidation";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                         if (sessionToken != null) headerParams.Add("sessionToken", ApiClient.ParameterToString(sessionToken)); // header parameter
 if (deviceId != null) headerParams.Add("deviceId", ApiClient.ParameterToString(deviceId)); // header parameter
 if (locale != null) headerParams.Add("locale", ApiClient.ParameterToString(locale)); // header parameter
                        postBody = ApiClient.Serialize(purchaseValidationRequest); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PurchaseValidation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PurchaseValidation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PurchaseValidationResponse) ApiClient.Deserialize(response.Content, typeof(PurchaseValidationResponse), response.Headers);
        }
    
        /// <summary>
        /// Refund an authorized transaction. Refund an authorized transaction. The content of the Refund Request will be kept in the Transaction Token that has been previously validated.
        /// </summary>
        /// <param name="transactionToken">This is the token obtain after a successful refund validation operation. It contains all the information that was submitted in the validation request.</param> 
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param> 
        /// <returns>RefundResponse</returns>            
        public RefundResponse Refund (string transactionToken, string locale)
        {
            
            // verify the required parameter 'transactionToken' is set
            if (transactionToken == null) throw new ApiException(400, "Missing required parameter 'transactionToken' when calling Refund");
            
    
            var path = "/payments/refund";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                         if (transactionToken != null) headerParams.Add("transactionToken", ApiClient.ParameterToString(transactionToken)); // header parameter
 if (locale != null) headerParams.Add("locale", ApiClient.ParameterToString(locale)); // header parameter
                            
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling Refund: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling Refund: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RefundResponse) ApiClient.Deserialize(response.Content, typeof(RefundResponse), response.Headers);
        }
    
        /// <summary>
        /// Perform a refund validation. Perform a refund validation. The element present in the request identifies the purchase transaction that need to be refunded. The result of this operation is the generation of a TransactionToken.
        /// </summary>
        /// <param name="sessionToken">This is the  token obtain after a successful authentication on the Admin API.</param> 
        /// <param name="deviceId">This element is what identifies the member in the Payment API. Each member can only have one deviceId active at the same time in the application. But because the refund operation is made by the Trendigo Backend (which is not a mobile device with a deviceId), the special constant TRENDIGO-BACKEND must be used. This will tell to the Payment API that the transaction does not originate from a mobile device.</param> 
        /// <param name="refundRequest"></param> 
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param> 
        /// <returns>RefundValidationResponse</returns>            
        public RefundValidationResponse RefundValidation (string sessionToken, string deviceId, RefundValidationRequest refundRequest, string locale)
        {
            
            // verify the required parameter 'sessionToken' is set
            if (sessionToken == null) throw new ApiException(400, "Missing required parameter 'sessionToken' when calling RefundValidation");
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling RefundValidation");
            
            // verify the required parameter 'refundRequest' is set
            if (refundRequest == null) throw new ApiException(400, "Missing required parameter 'refundRequest' when calling RefundValidation");
            
    
            var path = "/payments/refundValidation";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                         if (sessionToken != null) headerParams.Add("sessionToken", ApiClient.ParameterToString(sessionToken)); // header parameter
 if (deviceId != null) headerParams.Add("deviceId", ApiClient.ParameterToString(deviceId)); // header parameter
 if (locale != null) headerParams.Add("locale", ApiClient.ParameterToString(locale)); // header parameter
                        postBody = ApiClient.Serialize(refundRequest); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling RefundValidation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling RefundValidation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RefundValidationResponse) ApiClient.Deserialize(response.Content, typeof(RefundValidationResponse), response.Headers);
        }
    
        /// <summary>
        /// Perform a void validation. Perform a void validation. The element present in the request identifies the void transaction that need to be voided. The result of this operation is the generation of a TransactionToken.
        /// </summary>
        /// <param name="sessionToken">This is the token obtain after a successful authentication on the Admin API.</param> 
        /// <param name="deviceId">This element is what identifies the clerk in the Payment API. Each clerk can only have one deviceId active at the same time in the application. A validation will be made to ensure that the clerk is voiding a transaction that was made by him.</param> 
        /// <param name="voidValidationRequest"></param> 
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR).</param> 
        /// <returns>VoidValidationResponse</returns>            
        public VoidValidationResponse VoidValidation (string sessionToken, string deviceId, VoidValidationRequest voidValidationRequest, string locale)
        {
            
            // verify the required parameter 'sessionToken' is set
            if (sessionToken == null) throw new ApiException(400, "Missing required parameter 'sessionToken' when calling VoidValidation");
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling VoidValidation");
            
            // verify the required parameter 'voidValidationRequest' is set
            if (voidValidationRequest == null) throw new ApiException(400, "Missing required parameter 'voidValidationRequest' when calling VoidValidation");
            
    
            var path = "/payments/voidValidation";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                         if (sessionToken != null) headerParams.Add("sessionToken", ApiClient.ParameterToString(sessionToken)); // header parameter
 if (deviceId != null) headerParams.Add("deviceId", ApiClient.ParameterToString(deviceId)); // header parameter
 if (locale != null) headerParams.Add("locale", ApiClient.ParameterToString(locale)); // header parameter
                        postBody = ApiClient.Serialize(voidValidationRequest); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling VoidValidation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling VoidValidation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VoidValidationResponse) ApiClient.Deserialize(response.Content, typeof(VoidValidationResponse), response.Headers);
        }
    
    }
}
