using System;
using System.Collections.Generic;

namespace ApiTest.Model
{
    public class Cars
    {
        public Cars()
        {
           
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Attribute
        {
            public string Name { get; set; }                         // "number_plate","kilometres","body_style","seats"
            public string DisplayName { get; set; }                 
            public string Value { get; set; }
            public string EncodedValue { get; set; }
        }

        public class AdditionalData
        {
            public List<object> BulletPoints { get; set; }
            public List<object> Tags { get; set; }
        }

        public class Member
        {
            public int MemberId { get; set; }
            public string Nickname { get; set; }
            public DateTime DateAddressVerified { get; set; }
            public DateTime DateJoined { get; set; }
            public int UniqueNegative { get; set; }
            public int UniquePositive { get; set; }
            public int FeedbackCount { get; set; }
            public bool IsAddressVerified { get; set; }
            public string Suburb { get; set; }
            public string Region { get; set; }
            public bool IsAuthenticated { get; set; }
        }

        public class ShippingOption
        {
            public int Type { get; set; }
            public int Price { get; set; }
            public string Method { get; set; }
            public int ShippingId { get; set; }
        }

        public class EmbeddedContent
        {
        }

        public class PaymentMethod
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class BoundaryImage
        {
            public object Thumbnail { get; set; }
            public object List { get; set; }
            public object Medium { get; set; }
            public object Gallery { get; set; }
            public object Large { get; set; }
            public object FullSize { get; set; }
            public int PhotoId { get; set; }
        }

        public class Root
        {
            public long ListingId { get; set; }                     
            public string Title { get; set; }
            public string Category { get; set; }
            public int StartPrice { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public object ListingLength { get; set; }
            public bool IsBold { get; set; }
            public int BidderAndWatchers { get; set; }
            public DateTime AsAt { get; set; }
            public string CategoryPath { get; set; }
            public int RegionId { get; set; }
            public string Region { get; set; }
            public int SuburbId { get; set; }
            public string Suburb { get; set; }
            public int ViewCount { get; set; }
            public DateTime NoteDate { get; set; }
            public string CategoryName { get; set; }
            public List<Attribute> Attributes { get; set; }
            public List<object> OpenHomes { get; set; }
            public string Subtitle { get; set; }
            public int MinimumNextBidAmount { get; set; }
            public bool IsOnWatchList { get; set; }
            public int WatchlistReminder { get; set; }
            public string PriceDisplay { get; set; }
            public AdditionalData AdditionalData { get; set; }
            public Member Member { get; set; }
            public string Body { get; set; }                        
            public List<object> Photos { get; set; }
            public int AllowsPickups { get; set; }
            public List<ShippingOption> ShippingOptions { get; set; }
            public string PaymentOptions { get; set; }
            public bool CanAddToCart { get; set; }
            public EmbeddedContent EmbeddedContent { get; set; }
            public bool SupportsQuestionsAndAnswers { get; set; }
            public bool SuppressComplementaryListings { get; set; }
            public List<PaymentMethod> PaymentMethods { get; set; }
            public BoundaryImage BoundaryImage { get; set; }
            public List<object> RenderOptions { get; set; }
            public int AreaOfBusiness { get; set; }
        }


    }
}
