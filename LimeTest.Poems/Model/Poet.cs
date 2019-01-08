using Newtonsoft.Json;

namespace Poems.Model
{
    public class Poet
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}