using Budoom;

namespace GoalSeek.API.Helpers.BudoomHelper
{
    internal class Calculation : IGoalSeek
    {
        private readonly ILogger _logger;
        private readonly decimal _calculatedFormulaInput;
        
        public Calculation(decimal calculatedFormulaInput, ILogger logger)
        {
            _calculatedFormulaInput = calculatedFormulaInput;
            _logger = logger;
        }
        public decimal Calculate(decimal x)
        {
            decimal result = 0;
            try
            {
                result = _calculatedFormulaInput * x;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApplicationException("BudoomHelper.Calculation.Calculate Error");
            }

            return result;
        }
    }
}
