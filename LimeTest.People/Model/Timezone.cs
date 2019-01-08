using Newtonsoft.Json;

namespace LimeTest.People.Model
{
    public class Timezone
    {
        [JsonProperty("offset")]
        public string Offset { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}