using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GymManagement.Api.Controllers
{
    public class ApiController : ControllerBase
    {
        public IActionResult Problem(List<Error> errors)
        {
            if (errors.Count is 0)
            {
                return Problem();  // return 500
            }

            if (errors.All(error => error.Type is ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            return Problem(errors[0]);
        }

        public IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, detail: error.Description);
        }

        public IActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            errors.ForEach(error =>
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description
                );
            });

            return ValidationProblem(modelStateDictionary);
        }
    }
}