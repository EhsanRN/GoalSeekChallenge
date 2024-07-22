using System.Text.Json.Serialization;

namespace GoalSeek.API.Models
{
    public class CalculationResult
    {
        public decimal TargetInput { get; set; }
        public int Iterations { get; set; }
        public bool IsGoalReached { get; set; }
    }
}
