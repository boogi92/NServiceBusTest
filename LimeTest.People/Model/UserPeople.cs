using System.Collections.Generic;
using Newtonsoft.Json;

namespace LimeTest.People.Model
{
    public class UserPeople
    {

        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }
    }
}