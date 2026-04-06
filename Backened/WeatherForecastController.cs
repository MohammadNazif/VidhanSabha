
using Microsoft.AspNetCore.Mvc;
using VidhanSabha.Application.Exceptions;

namespace VidhanSabha.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet("throw-validation")]
        public IActionResult ThrowValidation()
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "Field1", new[] { "Field1 is required." } }
            });
        }

        [HttpGet("throw-notfound")]
        public IActionResult ThrowNotFound()
        {
            throw new NotFoundException("Resource not found.");
        }

        [HttpGet("throw-unauthorized")]
        public IActionResult ThrowUnauthorized()
        {
            throw new UnauthorizedException("Unauthorized access.");
        }

        [HttpGet("throw-forbidden")]
        public IActionResult ThrowForbidden()
        {
            throw new ForbiddenException("Forbidden access.");
        }

        [HttpGet("throw-argument")]
        public IActionResult ThrowArgument()
        {
            throw new ArgumentException("Invalid argument.");
        }

        [HttpGet("throw-generic")]
        public IActionResult ThrowGeneric()
        {
            throw new Exception("Generic error.");
        }
    }
}