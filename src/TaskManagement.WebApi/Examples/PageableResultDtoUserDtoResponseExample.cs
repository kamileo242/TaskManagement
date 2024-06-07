using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class PageableResultDtoUserDtoResponseExample : IExamplesProvider<PageableResultDto<UserDto>>
  {
    public PageableResultDto<UserDto> GetExamples()
    {
      var result = new PageableResultDto<UserDto>()
      {
        Items = new UserDto[]
        {
          new UserDto()
          {
            Id = "d8e2d6d5a65e4ad09967bd8ae50c2eea",
            Name = "Kamil",
            Surname = "Wiśniewski",
            Email = "KWisniewski@gmail.com",
            Position = "Junior .NET Developer",
            PhoneNumber = "123456789",
          },
          new UserDto
          {
            Id = "82v490c15e094e7aca11f06Ee14b69c9",
            Name = "Mariusz",
            Surname = "Kowalski",
            Email = "MKowalski@gmail.com",
            Position = "Senior .NET Developer",
            PhoneNumber = "123789456",
          },
          new UserDto
          {
            Id = "a4472c986705467c96b041d379067fd0",
            Name = "Michał",
            Surname = "Solecki",
            Email = "MSolecki24219@gmail.com",
            Position = "Mid .NET Developer",
            PhoneNumber = "789456123",
          },
        },

        Pagination = new PaginationDto { PageNumber = 0, PageSize = 50, TotalElements = 3 },
      };

      return result;
    }
  }
}
