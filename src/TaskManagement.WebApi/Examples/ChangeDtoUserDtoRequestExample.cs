using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class ChangeDtoUserDtoRequestExample : IExamplesProvider<ChangeDto<PatchUserDto>>
  {
    public ChangeDto<PatchUserDto> GetExamples()
      => new()
      {
        Data = new()
        {
          Name = "Kamil",
          Surname = "Wiśniewski",
          Email = "Kamileo24219@gmail.com",
          Position = "Junior .NET Developer",
          PhoneNumber = "513107186",
        }
      };
  }
}
