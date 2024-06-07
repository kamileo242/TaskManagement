using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class TeamDtoRequestExample : IExamplesProvider<StoreTeamDto>
  {
    public StoreTeamDto GetExamples()
      => new()
      {
        Name = "NET",
      };
  }
}
