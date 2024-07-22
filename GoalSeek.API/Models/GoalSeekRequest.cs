using GoalSeek.API.Validations;
using GoalSeek.API.Validations.JsonConverters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace GoalSeek.API.Models
{
    public class GoalSeekRequest
    {
        [Required(ErrorMessage ="Formula is required.")]
        [JsonConverter(typeof(FormulaConverter))]
        [FormulaValidator]
        //could not find correct expression, closest => [RegularExpression(@"[-+]?([0-9]*\.)?[0-9]+([eE][-+]?[0-9]+)?[\*\+/-]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?", ErrorMessage = "Invalid Formula")]
        public string Formula { get; set; }

        [Required(ErrorMessage = "Input is required.")]
        //[RegularExpression(@"^[-+]?[0-9]*[.]?[0-9]+([eE][-+]?[0-9]+)?", ErrorMessage = "Invalid Input")]
        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Invalid Input")]
        public float Input { get; set; }

        [Required(ErrorMessage = "TargetResult is required.")]
        //[RegularExpression(@"^[-+]?[0-9]*[.]?[0-9]+([eE][-+]?[0-9]+)?", ErrorMessage = "Invalid TargetResult")]
        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Invalid TargetResult")]
        public float TargetResult { get; set; }

        [Required(ErrorMessage = "MaximumIterations is required.")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Invalid MaximumIterations")]
        public int MaximumIterations { get; set; }
    }
}
