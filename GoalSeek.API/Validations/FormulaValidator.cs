using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;

namespace GoalSeek.API.Validations
{
    public class FormulaValidator: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = true;
            if(value == null)
            {
                result = false;
            }
            try
            {
                //calculate formula string 
                DataTable dt = new DataTable();
                dt.Compute(value.ToString(), "")?.ToString();
            }
            catch (Exception ex)
            {
                //only checking for formula syntax to be computable
                if (ex.GetType().Equals(typeof(SyntaxErrorException)))
                {
                    base.ErrorMessage = "Entered formula is invalid!";

                    result = false;
                }
            }

            return result;
        }
    }
}
