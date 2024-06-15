using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class PageableResultDtoProjectDtoResponseExample : IExamplesProvider<PageableResultDto<ProjectDto>>
  {
    public PageableResultDto<ProjectDto> GetExamples()
    {
      var result = new PageableResultDto<ProjectDto>()
      {
        Items = new ProjectDto[]
        {
          new ProjectDto()
          {
            Id = "d8e2d6d5a65e4ad09967bd8ae50c2eea",
            Title = "Utworzenie aplikacji do zarządzania projektami",
            Description = "Aplikacja powinna zawierać takie encje jak Użytkownik, Projekt, Zadanie, Historia zmian oraz Zespół",
            Deadline = DateTime.Parse("2024-10-01"),
            Status = Models.Statueses.ProjectStatus.Started.Value,
            CreatedAt = DateTime.Parse("2024-02-19"),
          },
          new ProjectDto
          {
            Id = "d8e2d6d5a65e4ad09967bd8ae50c2eea",
            Title = "Utworzenie biblioteki z filtrami",
            Description = "Biblioteka powinna umożliwiać wyszukiwanie obiektów wraz z sortowaniem i paginacja oraz obsługiwać filtry Or oraz And",
            Deadline = DateTime.Parse("2024-10-01"),
            Status = Models.Statueses.ProjectStatus.Started.Value,
            CreatedAt = DateTime.Parse("2024-02-19"),
          }
        },

        Pagination = new PaginationDto { PageNumber = 0, PageSize = 50, TotalElements = 2 },
      };

      return result;
    }
  }
}
