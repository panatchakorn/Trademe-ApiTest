using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTest.Utils
{
    public class Settings
    {
        public static readonly string baseUrl = $"https://api.tmsandbox.co.nz";
      
        // Initial request url for token (temp token)
        public static readonly string oauthBaseUrl = $"https://secure.tmsandbox.co.nz";
        public static readonly string oauthRequestTokenUrl = $"/Oauth/RequestToken?scope=MyTradeMeRead,MyTradeMeWrite";

        // After initial request for temp token, use this url to authenticate user
        public static readonly string oauthAuthoriseUserUrl = $"/Oauth/Authorize?oauth_token=";

        // After user has authenticated, use this url to get final token
        public static readonly string oauthFinalTokenUrl = $"/Oauth/AccessToken";

        //TRademe requires the value to be "oob" when not providing callback url
        public static readonly string oauth_callback = "oob";

       //Key for PJTestApp
        public static readonly string oauth_consumer_key = "EE88DB77329094ECBAFB716DDD448FC9"; 
        public static readonly string oauth_signature = Uri.EscapeDataString("F7A409C7C9C923F8EBE5642F313245F5&"); 
        
        // This is signature method require when request for token
        public static readonly string oauth_signature_method_token = "PLAINTEXT";
       
        // This is signature method require when call resources
        public static readonly string oauth_signature_method_resource = "PLAINTEXT";
        
    }
}
