using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class UserDtoRequestExample : IExamplesProvider<StoreUserDto>
  {
    public StoreUserDto GetExamples()
      => new()
      {
        Name = "Kamil",
        Surname = "Wiśniewski",
        Email = "Kamileo24219@gmail.com",
        Position = "Junior .NET Developer",
        PhoneNumber = "513107186",
      };
  }
}
