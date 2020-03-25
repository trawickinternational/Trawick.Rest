using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Trawick.Data.Models;

namespace Trawick.Rest.Code
{
    public class APIHelper: Trawick.Data.Models.APIRequest.IApiRequestProcessor 
    {
        public APIHelper()
        { }
        public  Trawick.Data.Models.OrderResponse ProcessApiRequest(string apiRequest)
        {
            var response = new Trawick.Data.Models.OrderResponse();

            string baseURL = System.Configuration.ConfigurationManager.AppSettings["API2017BaseUrl"];

            var PostE = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PostEcommerce"]);
            if (PostE)
                 apiRequest += "&bypassAnalytics=True";

            Uri requestURI = new Uri(baseURL);
            WebClient request = new WebClient();
            string requestParams = HttpUtility.HtmlDecode(apiRequest);
            var apiReq = new Trawick.Data.Models.ApiRequest(requestParams);
            requestParams = apiReq.ParseRequest();
            byte[] requestBody = Encoding.UTF8.GetBytes(requestParams);

            request.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                byte[] postResponse = request.UploadData(requestURI, "POST",requestBody);
                string jsonResponse = System.Text.Encoding.Default.GetString(postResponse);
                response = JsonConvert.DeserializeObject<Trawick.Data.Models.OrderResponse>(jsonResponse);

            }

            catch (WebException ex)
            {
                response.OrderStatusCode = -999;
            }

            return response;
        }

        public QuoteResponse ProcessQuoteRequest(string request)
        {
            string baseURL = System.Configuration.ConfigurationManager.AppSettings["API2017BaseUrl"];

            Uri requestURI = new Uri(baseURL);
            WebClient requestClient = new WebClient();

            var apiReq = new Trawick.Data.Models.ApiRequest(request);
            request = apiReq.ParseRequest();
            byte[] requestBody = Encoding.UTF8.GetBytes(request);
            requestClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                byte[] postResponse = requestClient.UploadData(requestURI, "POST", requestBody);
                string jsonResponse = System.Text.Encoding.Default.GetString(postResponse);
                var response = JsonConvert.DeserializeObject<Trawick.Data.Models.QuoteResponse>(jsonResponse);


                return response;

            }
            catch (WebException ex)
            {
                //do something
            }
            return new QuoteResponse() { };
        }
    }
}