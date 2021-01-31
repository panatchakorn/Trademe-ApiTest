using Newtonsoft.Json;

namespace ApiTest.Model
{

    public class Charities
    {
        public Charities()
        {

        }
    
        [JsonProperty("CharityType")]
        public string CharityType { get; set; }
        [JsonProperty("ImageSource")]
        public string ImageSource { get; set; }
        [JsonProperty("DarkModeImageSource")]
        public string DarkModeImageSource { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Tagline")]
        public string Tagline { get; set; }

    }
}
