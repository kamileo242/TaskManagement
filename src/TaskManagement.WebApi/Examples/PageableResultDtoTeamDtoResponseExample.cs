using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class PageableResultDtoTeamDtoResponseExample : IExamplesProvider<PageableResultDto<TeamDto>>
  {
    public PageableResultDto<TeamDto> GetExamples()
    {
      var result = new PageableResultDto<TeamDto>()
      {
        Items = new TeamDto[]
        {
          new TeamDto()
          {
            Id = "d8e2d6d5a65e4ad09967bd8ae50c2eea",
            Name = "NET",
          },
          new TeamDto
          {
            Id = "82v490c15e094e7aca11f06Ee14b69c9",
            Name = "Java",
          }
        },

        Pagination = new PaginationDto { PageNumber = 0, PageSize = 50, TotalElements = 2 },
      };

      return result;
    }
  }
}
