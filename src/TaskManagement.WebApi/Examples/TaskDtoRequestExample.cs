using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class TaskDtoRequestExample : IExamplesProvider<StoreTaskDto>
  {
    public StoreTaskDto GetExamples()
      => new()
      {
        Title = "Implementacja historii zmian",
        Description = "Zaimplementowanie historii zmian zapisującej operacje wykonywane na wszytskich ewidencjach",
        Priority = "normalny",
        Deadline = DateTime.Parse("2024-04-01"),
      };
  }
}
