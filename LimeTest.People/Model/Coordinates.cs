using Newtonsoft.Json;

namespace LimeTest.People.Model
{
    public class Coordinates
    {
        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}