using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class TeamDtoResponseExample : IExamplesProvider<TeamDto>
  {
    public TeamDto GetExamples()
      => new()
      {
        Id = "a5f054562a034e3d916c450fae09877b",
        Name = "NET",
      };
  }
}
