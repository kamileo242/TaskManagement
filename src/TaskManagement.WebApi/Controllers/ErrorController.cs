using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WebApi.Controllers
{
  [ApiController]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class ErrorController : ControllerBase
  {
    private readonly ILogger<ErrorController> logger;

    private record Result(HttpStatusCode StatusCode, string Title, string Details = null, bool Log = false);

    public ErrorController(ILogger<ErrorController> logger)
    {
      this.logger = logger;
    }

    [Route("/error")]
    public IActionResult ErrorInProductionEnvironment()
      => GetResponse(isDevelopment: false);

    [Route("/error-development")]
    public IActionResult ErrorInDevelopmentEnvironment()
      => GetResponse(isDevelopment: true);

    private ObjectResult GetResponse(bool isDevelopment)
    {
      var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
      var exception = context.Error;

      var result = exception switch
      {
        InvalidDataException => new Result(HttpStatusCode.BadRequest, "Nieprawidłowe dane", exception.Message),
        JsonException => new Result(HttpStatusCode.BadRequest, "Błąd podczas deserializacji danych JSON", exception.Message),
        _ when isDevelopment => new Result(HttpStatusCode.InternalServerError, exception.Message, exception.StackTrace, true),
        _ => new Result(HttpStatusCode.InternalServerError, "Wewnętrzny błąd usługi", exception.Message, true),
      };

      if (result.Log)
      {
        logger.LogError($"Error in {context.Path}. {exception.Message}. Stack Trace: {exception.StackTrace}");
      }

      return Problem(
        statusCode: (int) result.StatusCode,
        title: result.Title,
        detail: result.Details,
        instance: context.Path);
    }
  }
}
