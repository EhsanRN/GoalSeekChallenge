using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoalSeek.API.Models
{
    public class GoalSeekResponse
    {
        public decimal? TargetInput { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Iterations { get; set; }

    }
}