using GoalSeek.API.Helpers;
using GoalSeek.API.Models;
using GoalSeek.API.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data;
using System.Net.Mime;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GoalSeek.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GoalSeekController : ControllerBase
    {
        private readonly ILogger<GoalSeekController> _logger;
        private readonly IGoalSeekRepository _repository;
        public GoalSeekController(ILogger<GoalSeekController> logger, IGoalSeekRepository goalSeekRepository)
        {
            _logger = logger;
            _repository = goalSeekRepository;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> Post(GoalSeekRequest model)
        {
            GoalSeekResponse response = null;
            try
            {
                if (ModelState.IsValid is false)
                {
                    LogModelStateErrors(ModelState);
                    return BadRequest("");
                }

                response = await _repository.ProcessCalculateAsync(model);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

            return Ok(response);
        }

        #region private methods
        private ActionResult HandleException(Exception ex)
        {
            if (ex.GetType().Equals(typeof(ApplicationException)) || ex.GetType().Equals(typeof(OverflowException)))
            {
                _logger.LogError($"GoalSeekController.Post tid: {Request?.HttpContext?.TraceIdentifier}, ApplicationException", ex);

                //return badrequest for identified application errors that are user input related
                return BadRequest("");
            }
            else
                _logger.LogError($"GoalSeekController.Post tid: {Request?.HttpContext?.TraceIdentifier}, Error Message: {ex.Message}, Stacktrace: {ex.StackTrace}", ex);


            return StatusCode(500,"");
        }
        private void LogModelStateErrors(ModelStateDictionary modelState)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };

            var errors = JsonSerializer.Serialize(modelState.ToDictionary(x =>x.Key, x=>x.Value?.Errors), options);
            _logger.LogInformation($"GoalSeekController.Post GoalSeekRequest {Request?.HttpContext?.TraceIdentifier}, Model Validation Errors: {errors}");
        }
        #endregion  
    }
}
