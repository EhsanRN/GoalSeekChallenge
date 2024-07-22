using GoalSeek.API.Helpers.BudoomHelper;
using GoalSeek.API.Models;

namespace GoalSeek.API.Helpers
{
    public class GoalSeekHelper
    {
        public static CalculationResult Calculate(GoalSeekRequestDto input, ILogger logger)
        {
            var newCalculation = new Calculation(input.FormulaComputedValue, logger);
            var goalSeek = new Budoom.GoalSeek(newCalculation);
            goalSeek.TrySeek(input.TargetValue, input.Guess, input.MaxIterations, false).Deconstruct(out decimal targetValue, out decimal accuracyLevel, out int iterations, out bool isGoalReached, out decimal closestValue);

            return new CalculationResult
            {
                TargetInput = closestValue,
                Iterations = iterations,
                IsGoalReached = isGoalReached
            };
        }
    }
}
