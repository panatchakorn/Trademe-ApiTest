using ApiTest.Model;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using static ApiTest.Utils.Settings;
using System.Net;

/* This class run SpecFlow api test that requires authentication.
 * eg. General Search, Used Car Search, View individual listing
 * 
 */

namespace ApiTest.Actions
{
    public class BaseApiOAuth1Test :TrademeLogin
    {
       
        public static void SetApiBaseUri()
        {
            Client = new RestClient(baseUrl);
           
        }
       
        public static void GeneralSearch(string keyword, string apiResource)
        {
            Request = new RestRequest(apiResource,Method.GET);
            //Request.Resource = apiResource;
            //Request.Method = Method.GET;
            Request.AddHeader("Authorization", headerApiRequest);
            Request.AddHeader("Accept", "application/json");
            Request.AddParameter("search_string", keyword);
            // Request.AddQueryParameter(keyword, apiResource);
            // Request.AddOrUpdateParameter("search_string".UrlEncode(), keyword.UrlEncode());
            // Request.AddOrUpdateParameter("search_string", keyword, ParameterType.QueryStringWithoutEncode);
            // Request.AddParameter("category", "0001-0268-");
            GetResponse();
            Console.WriteLine(">>> Response: "+ Response.Content.ToString());
          
        }
        public static void ViewListingDetails(Int64 listingId, string apiResource)
        {
            Request = new RestRequest(apiResource + "/" + listingId + ".json",Method.GET);
           // Request.Resource = apiResource + "/"+ listingId + ".json";
           // Request.Method = Method.GET;
            Request.AddHeader("Authorization", headerApiRequest);
            Request.AddHeader("Accept", "application/json");
           
            GetResponse();
            
            //Console.WriteLine(Response.Content.ToString());
            
        }

        public static void AllUsedCarSearch(string apiResource)
        {
            Request = new RestRequest(apiResource,Method.GET);
           // Request.Resource = apiResource;
           // Request.Method = Method.GET;
            Request.AddHeader("Authorization", headerApiRequest);
            Request.AddHeader("Accept", "application/json");
           // Request.AddParameter("search_string", keyword);
            GetResponse();
            Console.WriteLine(">>> Response: " + Response.Content.ToString());
            
        }

        public static T DeserialiseResponse<T>()
        {
            JsonDeserializer jsonDeserializer = new JsonDeserializer();
            return jsonDeserializer.Deserialize<T>(Response);
        }

        //This method checks response status code
        public static Boolean CheckStatusCode(HttpStatusCode status)
        {
           // Console.WriteLine(">>> Actual Status Code: " + Response.StatusCode);
            if (Response.StatusCode == status)
                return true;
            return false;
        }

        // This method make sure the car listing contain the expected NumberPlate, Km, Body and Seats
        public static Boolean CheckCarDetails(Int64 listingId, string numberPlate, string kilometres, string body, string seats)
        {
            var result = DeserialiseResponse<List<Cars.Root>>();
            string resultNumberPlate = "";
            string resultKm = ""; 
            string resultBody = ""; 
            string resultSeats = "";


            for (int i = 0; i < result[0].Attributes.Count; i++)
            {
                if (result[0].Attributes[i].Name == "number_plate")
                    resultNumberPlate = result[0].Attributes[i].Value;
                else if (result[0].Attributes[i].Name == "kilometres")
                    resultKm = result[0].Attributes[i].Value;
                else if (result[0].Attributes[i].Name == "body_style")
                    resultBody = result[0].Attributes[i].Value;
                else if (result[0].Attributes[i].Name == "seats")
                    resultSeats = result[0].Attributes[i].Value;
            }
            /*
            Console.WriteLine(">>> Actual Number Plate: " + resultNumberPlate);
            Console.WriteLine(">>> Actual Kilometres: " + resultKm);
            Console.WriteLine(">>> Actual Body: " + resultBody);
            Console.WriteLine(">>> Actual Seats: " + resultSeats);
            */

            if (result[0].ListingId.Equals(listingId) && resultNumberPlate.Equals(numberPlate)
                && resultKm.Equals(kilometres) && resultBody.Equals(body)
                && resultSeats.Equals(seats))
                return true;
            return false;
        }
    }
}
