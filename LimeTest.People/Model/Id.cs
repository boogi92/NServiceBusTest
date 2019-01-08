using Newtonsoft.Json;

namespace LimeTest.People.Model
{
    public class Id
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}