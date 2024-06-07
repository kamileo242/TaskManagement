using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class ProjectDtoResponseExample : IExamplesProvider<ProjectDto>
  {
    public ProjectDto GetExamples()
      => new()
      {
        Id = "d8e2d6d5a65e4ad09967bd8ae50c2eea",
        Title = "Utworzenie aplikacji do zarządzania projektami",
        Description = "Aplikacja powinna zawierać takie encje jak Użytkownik, Projekt, Zadanie, Historia zmian oraz Zespół",
        Priority = 1,
        Deadline = DateTime.Parse("2024-10-01"),
        Status = Models.Statueses.ProjectStatus.Started.Value,
        CreatedAt = DateTime.Parse("2024-02-19"),
      };
  }
}
