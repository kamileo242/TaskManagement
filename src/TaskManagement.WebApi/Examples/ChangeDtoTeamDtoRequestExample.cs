using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class ChangeDtoTeamDtoRequestExample : IExamplesProvider<ChangeDto<PatchTeamDto>>
  {
    public ChangeDto<PatchTeamDto> GetExamples()
      => new()
      {
        Data = new()
        {
          Name = "NET"
        }
      };
  }
}
