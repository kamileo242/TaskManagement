using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class ChangeDtoTaskDtoRequestExample : IExamplesProvider<ChangeDto<PatchTaskDto>>
  {
    public ChangeDto<PatchTaskDto> GetExamples()
      => new()
      {
        Data = new()
        {
          Title = "Implementacja historii zmian",
          Description = "Zaimplementowanie historii zmian zapisującej operacje wykonywane na wszytskich ewidencjach",
          Priority = 1,
          Deadline = DateTime.Parse("2024-04-01"),
        }
      };
  }
}
