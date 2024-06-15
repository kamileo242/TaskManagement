using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class ProjectDtoRequestExample : IExamplesProvider<StoreProjectDto>
  {
    public StoreProjectDto GetExamples()
      => new()
      {
        Title = "Utworzenie aplikacji do zarządzania projektami",
        Description = "Aplikacja powinna zawierać takie encje jak Użytkownik, Projekt, Zadanie, Historia zmian oraz Zespół",
        Deadline = DateTime.Parse("2024-10-01")
      };
  }
}
