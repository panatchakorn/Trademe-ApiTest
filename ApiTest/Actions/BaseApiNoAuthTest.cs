using System;
using System.Collections.Generic;
using System.Linq;
using ApiTest.Model;
using RestSharp;
using RestSharp.Serialization.Json;
using static ApiTest.Utils.Settings;

/* This class run SpecFlow api test that no require authentication
 *  eg. Get Charities List
 * 
 */
namespace ApiTest.Actions
{
    public class BaseApiNoAuthTest
    {
        public static RestClient Client;
        public static IRestRequest Request;
        public static IRestResponse Response;
    

        public static void SetBaseUri()
        {
            Client = new RestClient(baseUrl);
           
        }

        public static void GetCharities(string apiResource)
        {
            Request = new RestRequest(apiResource);
            //Request.Resource = apiResource;
            GetResponse();
        }

        public static void GetResponse()
        {
            Response = Client.Execute(Request);
          // Response.Content.Contains("St John"); // can be uncommented to do a quick check that response has St John
        }

        public static T DeserialiseResponse<T>()
        {
            JsonDeserializer jsonDeserializer = new JsonDeserializer();
            return jsonDeserializer.Deserialize<T>(Response);
        }
        
        public static Boolean CheckCharities(string charity)
        {
            var result = DeserialiseResponse<List<Charities>>();
            
            
            
            int totalRecords = result.Count();
            for (int i = 0; i < totalRecords; i++)
            {
                //Assert.That(result[i].Description, Is.EqualTo(charity)); // can be uncommented to check the result
                if (result[i].Description.Equals(charity))
                     return true;                 
            }
            return false;
        }
    }
}
