using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class UserDtoResponseExample : IExamplesProvider<UserDto>
  {
    public UserDto GetExamples()
      => new()
      {
        Id = "d8e2d6d5a65e4ad09967bd8ae50c2eea",
        Name = "Kamil",
        Surname = "Wiśniewski",
        Email = "Kamileo24219@gmail.com",
        Position = "Junior .NET Developer",
        PhoneNumber = "513107186",
      };
  }
}
