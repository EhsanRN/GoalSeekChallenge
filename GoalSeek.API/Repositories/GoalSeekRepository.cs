using GoalSeek.API.Helpers;
using GoalSeek.API.Helpers.BudoomHelper;
using GoalSeek.API.Models;
using System.Data;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace GoalSeek.API.Repositories
{
    public class GoalSeekRepository : IGoalSeekRepository
    {
        private readonly ILogger _logger;

        public GoalSeekRepository(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<GoalSeekResponse> ProcessCalculateAsync(GoalSeekRequest model)
        {
            var dto = new GoalSeekRequestDto
                            {
                                Formula = model.Formula,
                                TargetValue = (decimal)model.TargetResult,
                                Guess = (decimal)model.Input,
                                MaxIterations = model.MaximumIterations
                            };

                var result = await TryCalculateAsync(dto);

            if (result is null)
            {
                throw new ApplicationException("ProcessCalculateAsync - No result");
            }
            return result;

        }

        #region private methods 
        private async Task<GoalSeekResponse> TryCalculateAsync(GoalSeekRequestDto input)
        {
            try 
            {
                //calculate formula string 
                DataTable dt = new DataTable();
                var computedValue = dt.Compute(input.Formula, "")?.ToString();

                if (computedValue is not null && decimal.TryParse(computedValue, out decimal formulaComputedValue))
                {
                    if (formulaComputedValue == 0)
                        throw new ApplicationException("TryCalculateAsync Error - Computed value of the formula cannot be zero.");

                    input.FormulaComputedValue = formulaComputedValue;

                    //perform calculation using helper algorithm
                    var calcResult = await Task.Run(() => GoalSeekHelper.Calculate(input, _logger));

                    return BuildResponse(calcResult);
                }
                else
                    throw new ApplicationException("TryCalculateAsync Error - Could not parse computed formula value to DECIMAL.");
            }
            catch (Exception ex) 
            {
                HandleException(ex);
                throw;
            }

            return null;

        }
        private GoalSeekResponse BuildResponse(CalculationResult calcResult)
        {
            GoalSeekResponse result = null;

            if (calcResult.IsGoalReached)
            {
                result = new GoalSeekResponse
                {
                    TargetInput = calcResult.TargetInput,
                    Iterations = calcResult.Iterations
                };
            }
            else
            {
                return new GoalSeekResponse
                {
                    TargetInput = null,
                };
            }

            return result;

        }
        private void HandleException(Exception ex)
        {
            _logger.LogError(ex.Message, ex);

            if (ex.GetType().Equals(typeof(EvaluateException)))
            {
                throw new ApplicationException("TryCalculateAsync Error - EvaluateException when evaluating formula input.");
            }
            if (ex is ApplicationException)
            {
                throw (ApplicationException)ex;
            }
        }

        #endregion
    }
}
