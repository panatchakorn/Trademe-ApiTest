using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authenticate
{
    public class Settings
    {
        // Initial request url for token (temp token)
        public static readonly string baseUrl = $"https://api.tmsandbox.co.nz";
        public static readonly string oauthBaseUrl = $"https://secure.tmsandbox.co.nz";
        public static readonly string oauthRequestTokenUrl = $"/Oauth/RequestToken?scope=MyTradeMeRead,MyTradeMeWrite";
        // After initial request for temp token, use this url to authenticate user
        public static readonly string oauthAuthoriseUserUrl = $"https://secure.tmsandbox.co.nz/Oauth/Authorize?oauth_token=";
        // After user us authenticated, use this url to get final token
        public static readonly string oauthFinalTokenUrl = $"https://secure.tmsandbox.co.nz/Oauth/AccessToken";

        public static readonly string oauth_callback = "oob";
        //public static readonly string oauth_consumer_key = "805C4B099A6F89CFF9B86A1ABFEFD452"; //Testapp
        public static readonly string oauth_consumer_key = "EE88DB77329094ECBAFB716DDD448FC9"; //PJTestApp
        // This is signature method require when request for token
        public static readonly string oauth_signature_method_token = "PLAINTEXT";
        // public static readonly var oauth_signature = Encoding.UTF8.GetBytes($"BB2114B38F3CFAFEC58A7F335304352B&");
        //public static readonly string oauth_signature = $"BB2114B38F3CFAFEC58A7F335304352B&";
        //public static readonly string oauth_signature = Uri.EscapeDataString("BB2114B38F3CFAFEC58A7F335304352B&"); //Testapp
        public static readonly string oauth_signature = Uri.EscapeDataString("F7A409C7C9C923F8EBE5642F313245F5&"); //PJTestApp
        // This is signature method require when call resources
        public static readonly string oauth_signature_method_resource = "PLAINTEXT";
        /*
        OAuth oauth_callback=oob,oauth_consumer_key=805C4B099A6F89CFF9B86A1ABFEFD452,oauth_signature_method=PLAINTEXT,oauth_signature=BB2114B38F3CFAFEC58A7F335304352B%26
            BB2114B38F3CFAFEC58A7F335304352B&
        */
               
    }
}
