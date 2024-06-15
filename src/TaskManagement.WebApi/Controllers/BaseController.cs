using Microsoft.AspNetCore.Mvc;
using Models;

namespace TaskManagement.WebApi.Controllers
{
  public abstract class BaseController : ControllerBase
  {
    /// <summary>
    /// Metoda przypisująca opis operacji do kontekstu.
    /// </summary>
    /// <param name="operationType"></param>
    /// <returns></returns>
    protected OperationContext CreateContext(string operationType)
    {
      return OperationContext.WithOperationType(operationType);
    }
  }
}
