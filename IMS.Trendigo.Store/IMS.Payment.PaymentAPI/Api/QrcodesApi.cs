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
    public interface IQrcodesApi
    {
        /// <summary>
        /// Perform a QR Code generation request. Perform a QR Code generation request. The result of this operation is a generated image in the format that was requested in the request. The QR Code will contain all the information that was pass in the request plus the location of the merchant (refer to locationInformation in this Admin API)
        /// </summary>
        /// <param name="sessionToken">This is the token obtain after a successful authentication on the Admin API.</param>
        /// <param name="deviceId"></param>
        /// <param name="qrcodeGenerationRequest"></param>
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR)</param>
        /// <returns></returns>
        byte[] QrcodeGeneration (string sessionToken, string deviceId, QrcodeGenerationRequest qrcodeGenerationRequest, string locale);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class QrcodesApi : IQrcodesApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QrcodesApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public QrcodesApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="QrcodesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public QrcodesApi(String basePath)
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
        /// Perform a QR Code generation request. Perform a QR Code generation request. The result of this operation is a generated image in the format that was requested in the request. The QR Code will contain all the information that was pass in the request plus the location of the merchant (refer to locationInformation in this Admin API)
        /// </summary>
        /// <param name="sessionToken">This is the token obtain after a successful authentication on the Admin API.</param> 
        /// <param name="deviceId"></param> 
        /// <param name="qrcodeGenerationRequest"></param> 
        /// <param name="locale">This will be used to select the languages in which the messages will be returned (possible values are : EN and FR)</param> 
        /// <returns></returns>            
        public byte[] QrcodeGeneration (string sessionToken, string deviceId, QrcodeGenerationRequest qrcodeGenerationRequest, string locale)
        {
            
            // verify the required parameter 'sessionToken' is set
            if (sessionToken == null) throw new ApiException(400, "Missing required parameter 'sessionToken' when calling QrcodeGeneration");
            
            // verify the required parameter 'deviceId' is set
            if (deviceId == null) throw new ApiException(400, "Missing required parameter 'deviceId' when calling QrcodeGeneration");
            
            // verify the required parameter 'qrcodeGenerationRequest' is set
            if (qrcodeGenerationRequest == null) throw new ApiException(400, "Missing required parameter 'qrcodeGenerationRequest' when calling QrcodeGeneration");
            
    
            var path = "/qrcodes";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
            if (sessionToken != null) headerParams.Add("apiKey", ApiClient.ParameterToString(sessionToken)); // header parameter
            if (deviceId != null) headerParams.Add("deviceId", ApiClient.ParameterToString(deviceId)); // header parameter
            if (locale != null) headerParams.Add("locale", ApiClient.ParameterToString(locale)); // header parameter
            postBody = ApiClient.Serialize(qrcodeGenerationRequest); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling QrcodeGeneration: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling QrcodeGeneration: " + response.ErrorMessage, response.ErrorMessage);
    
            return response.RawBytes;
        }
    
    }
}
