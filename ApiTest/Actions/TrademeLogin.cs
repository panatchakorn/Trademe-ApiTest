using System;
using System.Linq;
using System.Windows.Forms;
using static ApiTest.Utils.Settings;
using RestSharp;
using System.Net;
using System.Web;
using Microsoft.VisualBasic;
using System.Net.Http;

/* This class authenticate user with Trademe and obtain Access Token and Secret using OAuth1
 * The access token is used by Specflow test only
 * 
 */

namespace ApiTest.Actions
{

    public class TrademeLogin
    {
        public static RestClient Client;
        public static IRestRequest Request;
        public static IRestResponse Response;
        public static string headerApiRequest;  //This will contain authorization header for api call after the authentication process is done.
        //private static object Inputbox;

        public static void Authenticate()
        {
            SetBaseUri();
            OAuth1();
        }
        public static void SetBaseUri()
        {
            Client = new RestClient(oauthBaseUrl);
        }
        // Open a browser and direct user to website. User will need to login, Allow app and retrieve Pin
        public static void GoToSite(string url)
        {
            System.Diagnostics.Process.Start(url);
        }
       // This call make request to api.
        public static void GetResponse()
        {
            try
            {
                Response = Client.Execute(Request);
                //Response = Client.ExecuteAsync(Request);
            }

            catch (HttpRequestException e)
            {
                Console.WriteLine("\n Exception Caught");
                Console.WriteLine("Message: { 0}", e.Message);
            }
        }

        public static void OAuth1()
        {

            string authResponseBody = "";
            string oauthTokenSecretTemp = "";
            string oauthTokenTemp = "";
            string oauthVerifierPin = "";
            string pinRetrivalUrl = "";
            string headerFinalToken = "";
            string oauthTokenSecretFinal = "";
            string oauthTokenFinal = "";

            //
            // REQUEST TEMP TOKEN
            //
            // Authorization header for temp token request
            string headerTempToken = "OAuth oauth_callback=" + oauth_callback + ",oauth_consumer_key=" + oauth_consumer_key
                              + ",oauth_signature_method=" + oauth_signature_method_token + ",oauth_signature=" + oauth_signature;

            Request = new RestRequest(oauthRequestTokenUrl, Method.POST);
            Request.AddHeader("Authorization", headerTempToken);
            Request.AddHeader("Content-Type", "text/html; charset=utf-8");

            GetResponse();

            var authResponseCode = Response.StatusCode;
            if (authResponseCode == HttpStatusCode.OK)
            {
                authResponseBody = Response.Content;

                if (authResponseBody.Contains("oauth_token_secret"))
                {
                    var t = HttpUtility.ParseQueryString(authResponseBody);
                    //Extract request token and secret
                    if (oauthTokenTemp == "")
                    {
                        oauthTokenTemp = t["oauth_token"];
                        oauthTokenSecretTemp = t["oauth_token_secret"];
                        Console.WriteLine(">>> Extracted Temp OAuthToken: " + oauthTokenTemp);
                        Console.WriteLine(">>> Extracted Temp OAuthToken Secret: " + oauthTokenSecretTemp);
                        // build url for authorising user access
                        pinRetrivalUrl = oauthAuthoriseUserUrl + oauthTokenTemp;
                    }
                }

            }
            else
            {
                Console.WriteLine("There is a problem getting temp token. Error code: " + authResponseCode.ToString());
                Console.WriteLine("Error details: " + authResponseBody.ToString());
            }

            //
            // GET USER AUTHORIZATION
            //
            // Send user to authenticate with Trademe and retrieve pin
            string userAuthenticationurl = oauthBaseUrl + pinRetrivalUrl;
            GoToSite(userAuthenticationurl);

            // Get verification pin from user. User has 3 tries.
            oauthVerifierPin = "";

            string pin;
            string attemptLeft = " try left!";
            //Console.WriteLine("Please enter 7 digit verification pin");
            pin = Interaction.InputBox("Clear the box and Enter your 7 digit pin." + '\n' + '\n' + "To get the pin, login to trademe from popup browser. If no browser pops up please manually copy below url and open it in a browser.", "Enter 7 digit PIN", userAuthenticationurl, 450, 400);
            pin = pin.Trim();

            for (int pinRetry = 2; pinRetry > 0; pinRetry--)
            {
                if (pin.Length == 7 && pin.All(char.IsDigit) == true)
                {
                    // verification pin is set
                    oauthVerifierPin = pin;
                    Console.WriteLine(">>> Extracted OAuth Verifier PIN: " + oauthVerifierPin);
                    break;
                }

                else if ((pin.Length != 7 || pin.All(char.IsDigit) == false) && pinRetry > 0)
                {
                    attemptLeft = pinRetry + " try left!";
                    pin = Interaction.InputBox("Clear the box and Enter your 7 digit pin." + '\n' + '\n' + "To get the pin, login to trademe from popup browser. If no browser pops up please manually copy below url and open it in a browser.", "Invalid format. " + attemptLeft + " Enter 7 digit PIN", userAuthenticationurl, 450, 400);
                }
            }

            if (oauthVerifierPin == "")
            {
                //MessageBox.Show("Too many invalid attempts. Good Bye!", "Exit");

                Console.WriteLine(">>> No OAuth Verifier PIN exit test execution!!");
                using (Form form = new Form { TopMost = true })
                {
                    form.TopLevel = true;
                    MessageBox.Show(form, "Too many invalid attempts. Good Bye!", "Exit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    form.Dispose();
                }

                System.Environment.Exit(0);

            }

            //
            // REQUEST ACCESS TOKEN (FINAL)
            //
            // Authorization header to get final token request
            headerFinalToken = "OAuth oauth_verifier=" + oauthVerifierPin + ",oauth_consumer_key=" + oauth_consumer_key
                + ",oauth_token=" + oauthTokenTemp + ",oauth_signature_method=" + oauth_signature_method_token + ",oauth_signature=" + oauth_signature + oauthTokenSecretTemp;

            Request = new RestRequest(oauthFinalTokenUrl, Method.POST);
            Request.AddHeader("Authorization", headerFinalToken);
            Request.AddHeader("Content-Type", "text/html; charset=utf-8");

            GetResponse();

            authResponseCode = Response.StatusCode;
            if (authResponseCode == HttpStatusCode.OK)
            {
                authResponseBody = Response.Content;
                if (authResponseBody.Contains("oauth_token_secret"))
                {
                    var t = HttpUtility.ParseQueryString(authResponseBody);
                    //Extract final token and secret
                    if (oauthTokenFinal == "")
                    {
                        oauthTokenFinal = t["oauth_token"];
                        oauthTokenSecretFinal = t["oauth_token_secret"];
                        Console.WriteLine(">>> Extracted Final OAuthToken: " + oauthTokenFinal);
                        Console.WriteLine(">>> Extracted Final OAuthToken Secret: " + oauthTokenSecretFinal);
                    }
                }
            }
            else
            {
                Console.WriteLine("There is a problem getting temp token. Error code: " + authResponseCode.ToString());
                Console.WriteLine("Error details: " + authResponseBody.ToString());
            }


            // Authorization header to for api request using final token and secret
            headerApiRequest = "OAuth oauth_consumer_key=" + oauth_consumer_key + ",oauth_token=" + oauthTokenFinal
               + ",oauth_signature_method=" + oauth_signature_method_resource + ",oauth_signature=" + oauth_signature + oauthTokenSecretFinal;

            Console.WriteLine(">>> Authentication process completed. <<<");
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        }
    }

}
