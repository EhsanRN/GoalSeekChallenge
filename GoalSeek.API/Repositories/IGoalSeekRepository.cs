using GoalSeek.API.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GoalSeek.API.Repositories
{
    public interface IGoalSeekRepository
    {
        Task<GoalSeekResponse> ProcessCalculateAsync(GoalSeekRequest model);
    }
}
