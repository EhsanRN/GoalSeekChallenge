namespace GoalSeek.API.Models
{
    public class GoalSeekRequestDto
    {
        public string Formula { get; set; }
        public decimal FormulaComputedValue { get; set; }
        public decimal TargetValue { get; set; }
        public decimal Guess { get; set; }
        public int MaxIterations { get; set; }
    }
}
