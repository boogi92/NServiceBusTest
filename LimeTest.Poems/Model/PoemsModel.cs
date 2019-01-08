using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Poems.Model
{

    public class PoemsModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("poet")]
        public Poet Poet { get; set; }

        [JsonIgnore]
        public string Author { get; set; }
    }
}