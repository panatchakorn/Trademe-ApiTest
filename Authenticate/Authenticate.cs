using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;
using RestSharp.Authenticators;

namespace Authenticate
{
    
    public class Authenticate
    {
        public static RestClient Client;
        public static IRestRequest Request;
        public static IRestResponse Response;

        public static void SetBaseUri()
        {
            Client = new RestClient(Settings.oauthBaseUrl);
           
        }

        public void TrademeAuthentication()
        {
            string authResponse = "";
            string oauthTokenSecretTemp = "";
            string oauthToken = "";
            string oauthVerifierPin = "";
            string pinRetrivalUrl = "";
            string headerFinalToken = "";
            string oauthResponseToken = "";
            string oauthTokenSecretFinal = "";
            string oauthToken2 = "";

            // Get request token
            //
            // Authorization header for temp token request
            string headerTempToken = "OAuth oauth_callback=" + Settings.oauth_callback + ",oauth_consumer_key=" + Settings.oauth_consumer_key
                              + ",oauth_signature_method=" + Settings.oauth_signature_method_token + ",oauth_signature=" + Settings.oauth_signature;
            
            authResponse = RequestToken(Settings.oauthRequestTokenUrl, headerTempToken);
            Console.WriteLine(authResponse);

            var qs = HttpUtility.ParseQueryString(authResponse.Content);
            var oauth_token = qs["oauth_token"];
            var oauth_token_secret = qs["oauth_token_secret"];

            requestToken.Token = oauth_token;
            requestToken.Secret = oauth_token_secret;

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
           
            var request = new RestRequest(Settings.oauthRequestTokenUrl, Method.POST);

            var response = client.Execute(request);
            /*
            if (response.StatusCode != HttpStatusCode.OK)
            {
                break;
            }

            OAuthToken requestToken = new OAuthToken();
            {
                var qs = HttpUtility.ParseQueryString(response.Content);
                var oauth_token = qs["oauth_token"];
                var oauth_token_secret = qs["oauth_token_secret"];

                requestToken.Token = oauth_token;
                requestToken.Secret = oauth_token_secret;
            }*/
        }

        private static string RequestToken(string requestUrl, string header)
        {
            //var Client = new RestClient(Settings.oauthBaseUrl);

            var authRequest = new RestRequest(requestUrl, Method.POST);
            var authHeader = header;
          
            authRequest.AddHeader("Authorization", authHeader);
            authRequest.AddHeader("Content-Type", "text/html; charset=utf-8");
            var authResponse = Client.Execute(authRequest);

            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var authResponseBody = authResponse.Content;
                //Console.Write(authResponseBody);
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
