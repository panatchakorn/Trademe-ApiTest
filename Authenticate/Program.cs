using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using RestSharp;
using RestSharp.Authenticators;

namespace Authenticate
{

    class Program
    {
        
        static void Main(string[] args)
        {
            /*
             string authResponse =  RequestToken();
                 Console.WriteLine(authResponse);
            */
            // Declare variables

            string authResponse = "";//"oauth_token=4FF4EFC8FE2C50C0D53D561F8B6C0A93&oauth_token_secret=782362A6325A9EA2FF0561CEA622EBF4&oauth_callback_confirmed=true";

            string oauthTokenSecretTemp = "";
            string oauthToken = "";
            string oauthVerifierPin = "";
            string pinRetrivalUrl = "";
            string headerFinalToken = "";
            string oauthResponseToken = "";
            string oauthTokenSecretFinal = "";
            string oauthToken2 = "";



            // Authorization header for temp token request
            string headerTempToken = "OAuth oauth_callback=" + Settings.oauth_callback + ",oauth_consumer_key=" + Settings.oauth_consumer_key
                              + ",oauth_signature_method=" + Settings.oauth_signature_method_token + ",oauth_signature=" + Settings.oauth_signature;

            // Step 1: Get Temp Token and secret         
            Console.WriteLine("STEP 1: Get Temp Token and secret");

            authResponse = RequestToken(Settings.oauthRequestTokenUrl,headerTempToken);
            Console.WriteLine(authResponse);

            if (authResponse.Contains("oauth_token_secret") == true)
            { 
                // Extract Token and Token Secret
                oauthResponseToken = authResponse;
                string[] oauthExtractResponseToken = oauthResponseToken.Split('&');
                foreach (string responseToken in oauthExtractResponseToken)
                {
                    Console.WriteLine(">> {0} ", responseToken);
                    //Console.WriteLine("split method before = {0} ", responseToken.Substring(0, responseToken.IndexOf("=")));
                    //Console.WriteLine("split method after = {0} ", responseToken.Substring(responseToken.IndexOf("=") + 1));
                    if (responseToken.Substring(0, responseToken.IndexOf("=")) == "oauth_token")
                    {
                        oauthToken = responseToken.Substring(responseToken.IndexOf("=") + 1);
                        Console.WriteLine(">> Temp Token Extracted", responseToken);
                    }
                        
                    else if (responseToken.Substring(0, responseToken.IndexOf("=")) == "oauth_token_secret")
                    {
                        oauthTokenSecretTemp = responseToken.Substring(responseToken.IndexOf("=") + 1);
                        Console.WriteLine(">> Temp Token Secret Extracted", responseToken);
                    }
                        
                }
            }
            else
            { 
                // There are some errors. Exit
                //Console.WriteLine(authResponse);
                Console.Write("Press <Enter> to exit... ");
                while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                //Thread.Sleep(10000);
                System.Environment.Exit(0);

            }

            // Step 2: Authorising access to user account.
            Console.WriteLine("STEP 2: Authorising access to user account");
            pinRetrivalUrl = Settings.oauthAuthoriseUserUrl + oauthToken;
            Console.WriteLine("Open this url in a browser and login to Trademe Sandbox.");
            Console.WriteLine(pinRetrivalUrl);
            Console.WriteLine("Click to ALLOW TestApp to access your account.");
            Console.WriteLine("The Verification pin should appears.");
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            // Get Verifier Pin and check the entered format is valid
            string pin;
            //int pinRetry = 3;
            string attemptLeft = " try left!";
            Console.WriteLine("Please enter 7 digit verification pin");
            pin = Console.ReadLine();
            //pinRetry = pinRetry - 1;
            
            for (int pinRetry = 2; pinRetry > 0; pinRetry--)
            {
                if (pin.Length == 7 && pin.All(char.IsDigit) == true)                    
                {
                    oauthVerifierPin = pin;
                    break;
                }

                else if ((pin.Length != 7 || pin.All(char.IsDigit) == false) && pinRetry > 0)
                {
                    attemptLeft = pinRetry + " try left!";
                    Console.WriteLine("Invalid pin, please enter 7 digit verification pin . You have " + attemptLeft);
                    pin = Console.ReadLine();
                }
            }

            // Exit when pin is not entered in the correct format
            if (oauthVerifierPin == "" || oauthVerifierPin is null)
            {
                Console.WriteLine("Too many invalid attempts.Good Bye");
                Console.Write("Press <Enter> to exit... ");
                while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                //Thread.Sleep(3000);
                System.Environment.Exit(0);
            }

            // Authorization header to get final token request
            headerFinalToken = "OAuth oauth_verifier=" + oauthVerifierPin + ",oauth_consumer_key=" + Settings.oauth_consumer_key
                + ",oauth_token=" + oauthToken + ",oauth_signature_method=" + Settings.oauth_signature_method_token + ",oauth_signature=" + Settings.oauth_signature + oauthTokenSecretTemp;
            
            // Step3: Get Final Token and secret
            authResponse = RequestToken(Settings.oauthFinalTokenUrl, headerFinalToken);
            Console.WriteLine(authResponse);

            if (authResponse.Contains("oauth_token_secret") == true && authResponse != null)
            {
                // Extract Final Token Secret
                oauthResponseToken = authResponse;
                string[] oauthExtractResponseTokenFinal = oauthResponseToken.Split('&');
                foreach (string responseToken in oauthExtractResponseTokenFinal)
                {
                    Console.WriteLine(">> {0} ", responseToken);
                    //Console.WriteLine("split method before = {0} ", responseToken.Substring(0, responseToken.IndexOf("=")));
                    //Console.WriteLine("split method after = {0} ", responseToken.Substring(responseToken.IndexOf("=") + 1));
                    if (responseToken.Substring(0, responseToken.IndexOf("=")) == "oauth_token")
                    oauthToken2 = responseToken.Substring(responseToken.IndexOf("=") + 1);
                    if (responseToken.Substring(0, responseToken.IndexOf("=")) == "oauth_token_secret")
                    {
                        oauthTokenSecretFinal = responseToken.Substring(responseToken.IndexOf("=") + 1);
                        Console.WriteLine(">> Final Token Secret Extracted", responseToken);
                    }
                        
                }
            }
            else
            {
                // There are some errors. Exit
                //Console.WriteLine(authResponse);
                Console.Write("Press <Enter> to exit... ");
                while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                //Thread.Sleep(10000);
                //System.Environment.Exit(0);
            }
           
            string headerApiRequest = "OAuth oauth_consumer_key=" + Settings.oauth_consumer_key + ",oauth_token=" + oauthToken2
                + ",oauth_signature_method=" + Settings.oauth_signature_method_resource + ",oauth_signature=" + Settings.oauth_signature + oauthTokenSecretFinal;

            Console.WriteLine("Header for service call: " + headerApiRequest);
            string requestserviceUrl = $"/v1/MyTradeMe/Watchlist/All.json";

            Console.WriteLine("***** Watchlist request ******");
            RequestWatchList(requestserviceUrl,headerApiRequest);

            requestserviceUrl = $"/v1/Listings/2149242890.json";

            Console.WriteLine("***** Single Listing request ******");
            RequestListing(requestserviceUrl, headerApiRequest);

            requestserviceUrl = $"/v1/Search/General.json";

            Console.WriteLine("***** General Search request ******");
            RequestGeneralSearch(requestserviceUrl, headerApiRequest);



            requestserviceUrl = $"/v1/Search/Motors/Used.xml";
            Console.WriteLine("***** Used Cards request ******");
            RequestUsedCarsSearch(requestserviceUrl, headerApiRequest);
            //string serviceResponse = RequestService(requestserviceUrl, headerApiRequest,oauthToken, oauthTokenSecretFinal);
            //Console.Write(serviceResponse);
            
            Console.Write("Press <Enter> to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
        // Send api request to authenticate with Trademe
        private static string RequestToken(string requestUrl , string header)
        {   
           var Client = new RestClient(Settings.oauthBaseUrl);
           
            var authRequest = new RestRequest(requestUrl, Method.POST);
            var authHeader = header;
            /*
            var authRequest = new RestRequest(Settings.oauthRequestTokenUrl, Method.POST);
            var authHeader = "OAuth oauth_callback=" + Settings.oauth_callback + ",oauth_consumer_key=" + Settings.oauth_consumer_key 
                              + ",oauth_signature_method=" + Settings.oauth_signature_method_token + ",oauth_signature=" + Settings.oauth_signature;
          */
            //var authHeader = "oauth_callback=" + oauth_callback + ",oauth_consumer_key=" + oauth_consumer_key + ",oauth_signature_method=" + oauth_signature_method_token + ",oauth_signature=" + oauth_signature;
            
            authRequest.AddHeader("Authorization", authHeader);
            //authRequest.AddHeader("Accept", "application/json");
            authRequest.AddHeader("Content_Type", "text/html; charset=utf-8");
            var authResponse = Client.Execute(authRequest);

            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var authResponseBody = authResponse.Content;
        // var authResponseBody = JObject.Parse(authResponse.Content);
        Console.Write(authResponseBody);
                //Console.WriteLine(JsonConvert.SerializeObject(Reponse.Content, Formatting.Indented));
                ////oauthToken = authResponseBody["oauth_token"].ToString();
                /// authTokenSecretTemp = authResponseBody["oauth_token_secret"].ToString();
                return Convert.ToString(authResponseBody);
            }
            else
            {
                //Console.WriteLine(authResponse.Content);
                return Convert.ToString(authResponse.Content);
            }
        }

        private static void RequestWatchList(string requestUrl, string header)
        {
            var Client = new RestClient(Settings.baseUrl);
            var authRequest = new RestRequest(requestUrl, Method.GET);
            var authHeader = header;
            authRequest.AddHeader("Authorization", authHeader);
            authRequest.AddHeader("Accept", "application/json");
            var authResponse = Client.Execute(authRequest);

            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var authResponseBody = authResponse.Content;
                // var authResponseBody = JObject.Parse(authResponse.Content);
                Console.WriteLine(authResponseBody.ToString());
                //Console.WriteLine(JsonConvert.SerializeObject(Reponse.Content, Formatting.Indented));
                ////oauthToken = authResponseBody["oauth_token"].ToString();
                /// authTokenSecretTemp = authResponseBody["oauth_token_secret"].ToString();
               // return Convert.ToString(authResponseBody);
            }
            else
            {
                Console.WriteLine(authResponse.Content.ToString());
               // return Convert.ToString(authResponse.Content);
            }
        }

        private static void RequestListing(string requestUrl, string header)
        {
            var Client = new RestClient(Settings.baseUrl);
            var authRequest = new RestRequest(requestUrl, Method.GET);
            var authHeader = header;
            authRequest.AddHeader("Authorization", authHeader);
            authRequest.AddHeader("Accept", "application/json");
            var authResponse = Client.Execute(authRequest);

            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var authResponseBody = authResponse.Content;
                // var authResponseBody = JObject.Parse(authResponse.Content);
                Console.WriteLine(authResponseBody.ToString());
                //Console.WriteLine(JsonConvert.SerializeObject(Reponse.Content, Formatting.Indented));
                ////oauthToken = authResponseBody["oauth_token"].ToString();
                /// authTokenSecretTemp = authResponseBody["oauth_token_secret"].ToString();
               // return Convert.ToString(authResponseBody);
            }
            else
            {
                Console.WriteLine(authResponse.Content.ToString());
                // return Convert.ToString(authResponse.Content);
            }
        }
        private static void RequestGeneralSearch(string requestUrl, string header)
        {
            var Client = new RestClient(Settings.baseUrl);
            var authRequest = new RestRequest(requestUrl, Method.GET);
            var authHeader = header;
            authRequest.AddHeader("Authorization", authHeader);
            authRequest.AddHeader("Content-Type", "application/json"); //"text/xml"
            //authRequest.AddHeader("Accept", "application/json");
            authRequest.AddHeader("search_string", "BMW");
            authRequest.AddHeader("category", "1");
            // authRequest.AddQueryParameter("search_string", "BMW",true);
            //authRequest.AddParameter("category", "1");
            var authResponse = Client.Execute(authRequest);

            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var authResponseBody = authResponse.Content;
                // var authResponseBody = JObject.Parse(authResponse.Content);
                Console.WriteLine(authResponseBody.ToString());
                //Console.WriteLine(JsonConvert.SerializeObject(Reponse.Content, Formatting.Indented));
                ////oauthToken = authResponseBody["oauth_token"].ToString();
                /// authTokenSecretTemp = authResponseBody["oauth_token_secret"].ToString();
               // return Convert.ToString(authResponseBody);
            }
            else
            {
                Console.WriteLine(authResponse.Content.ToString());
                // return Convert.ToString(authResponse.Content);
            }
        }

        private static void RequestUsedCarsSearch(string requestUrl, string header)
        {
            var Client = new RestClient(Settings.baseUrl);
            var authRequest = new RestRequest(requestUrl, Method.GET);
            var authHeader = header;
            authRequest.AddHeader("Authorization", authHeader);
            authRequest.AddHeader("Content-Type", "text/xml"); //"text/xml"
           // authRequest.AddHeader("Content-Type", "application/json"); //"text/xml"

            //authRequest.AddHeader("Accept", "application/json");
            //authRequest.AddHeader("54", "BMW");  //search_string
            // authRequest.AddQueryParameter("search_string", "BMW",true);
             //authRequest.AddQueryParameter("54", "BMW",true);
            //authRequest.AddParameter("category", "1");
            var authResponse = Client.Execute(authRequest);

            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var authResponseBody = authResponse.Content;
                // var authResponseBody = JObject.Parse(authResponse.Content);
                Console.WriteLine(authResponseBody.ToString());
                //Console.WriteLine(JsonConvert.SerializeObject(Reponse.Content, Formatting.Indented));
                ////oauthToken = authResponseBody["oauth_token"].ToString();
                /// authTokenSecretTemp = authResponseBody["oauth_token_secret"].ToString();
               // return Convert.ToString(authResponseBody);
            }
            else
            {
                Console.WriteLine(authResponse.Content.ToString());
                // return Convert.ToString(authResponse.Content);
            }
        }
        // Send api request to retrieve information
        private static string RequestService(string requestUrl, string header, string oauthT, string osecret)
        {
            /*
            var baseAddress = new Uri("https://www.tmsandbox.co.nz");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                //usually i make a standard request without authentication, eg: to the home page.
                //by doing this request you store some initial cookie values, that might be used in the subsequent login request and checked by the server
                var homePageResult = client.GetAsync("/");
                homePageResult.Result.EnsureSuccessStatusCode();

                var content = new FormUrlEncodedContent(new[]
                {
        //the name of the form values must be the name of <input /> tags of the login form, in this case the tag is <input type="text" name="username">
        new KeyValuePair<string, string>("page_email", "panatchakorn@gmail.com"),
        new KeyValuePair<string, string>("page_password", "JCpj77!7"),
         new KeyValuePair<string, string>("login_attempts", "0"),
          new KeyValuePair<string, string>("submitted", "Y"),
           new KeyValuePair<string, string>("validationToken", "ccb79153-6b45-4cce-a74a-1fa9ef8f23da"),
            new KeyValuePair<string, string>("has_javascript", "1"),
    });
                var loginResult = client.PostAsync("/Members/SecureLogin.aspx", content).Result;
                loginResult.EnsureSuccessStatusCode();

                //make the subsequent web requests using the same HttpClient object
            }

            */

            var Client = new RestClient(Settings.baseUrl);
            
            //Client.BaseUrl = Settings.baseUrl;
           
            var authRequest = new RestRequest(requestUrl, Method.GET);
            var authHeader = header;
            /*
            var authRequest = new RestRequest(Settings.oauthRequestTokenUrl, Method.POST);
            var authHeader = "OAuth oauth_callback=" + Settings.oauth_callback + ",oauth_consumer_key=" + Settings.oauth_consumer_key 
                              + ",oauth_signature_method=" + Settings.oauth_signature_method_token + ",oauth_signature=" + Settings.oauth_signature;
            */
            //var authHeader = "oauth_callback=" + oauth_callback + ",oauth_consumer_key=" + oauth_consumer_key + ",oauth_signature_method=" + oauth_signature_method_token + ",oauth_signature=" + oauth_signature;
          
          
            //authRequest.AddHeader("Accept", "application/json");
           
            authRequest.AddHeader("Authorization", authHeader);
            // Client.Authenticator = OAuth1Authenticator.ForClientAuthentication(oauthT, osecret, "panatchakorn@gmail.com", "JCpj77!7");
            // Client.Authenticator = OAuth1Authenticator.ForProtectedResource(Settings.oauth_consumer_key, Settings.oauth_signature+osecret, oauthT, osecret);
             //authRequest.AddHeader("Accept", "application/json");
            authRequest.AddHeader("Content-Type", "application/json");
            //authRequest.AddHeader("Content_Type", "text/html; charset=utf-8");

            authRequest.AddHeader("odometer_max", "200000");
            authRequest.AddHeader("search_string", "BMW");
            /*
            authRequest.AddHeader("body_style", "");
            authRequest.AddHeader("condition", "");
            authRequest.AddHeader("date_from", "");
            authRequest.AddHeader("doors_max", "");
            authRequest.AddHeader("doors_min", "");
            authRequest.AddHeader("engine_size_max","");
            authRequest.AddHeader("engine_size_min", "");
            authRequest.AddHeader("listed_as", "");
            authRequest.AddHeader("listing_type", "");
            authRequest.AddHeader("make", "");
            authRequest.AddHeader("member_listing", "");
            authRequest.AddHeader("model", "");
            authRequest.AddHeader("odometer_max", "");
            authRequest.AddHeader("odometer_min", "");
            authRequest.AddHeader("page", "");
            authRequest.AddHeader("photo_size", "");
            authRequest.AddHeader("price_max", "");
            authRequest.AddHeader("price_mnin", "");
            authRequest.AddHeader("rows", "");
            authRequest.AddHeader("search_string", "");
            authRequest.AddHeader("seats_max", "");
            authRequest.AddHeader("seats_min", "");
            authRequest.AddHeader("shipping_method", "");
            authRequest.AddHeader("sprt_order", "");
            authRequest.AddHeader("transmission", "");
            authRequest.AddHeader("user_region", "");
            authRequest.AddHeader("year_max", "");
            authRequest.AddHeader("year_min", "");
            */

            var authResponse = Client.Execute(authRequest);

            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var authResponseBody = authResponse.Content;
                // var authResponseBody = JObject.Parse(authResponse.Content);
                Console.Write(authResponseBody);
                //Console.WriteLine(JsonConvert.SerializeObject(Reponse.Content, Formatting.Indented));
                ////oauthToken = authResponseBody["oauth_token"].ToString();
                /// authTokenSecretTemp = authResponseBody["oauth_token_secret"].ToString();
                return Convert.ToString(authResponseBody);
            }
            else
            {
                //Console.WriteLine(authResponse.Content);
                return Convert.ToString(authResponse.Content);
            }
        }




    }
}
