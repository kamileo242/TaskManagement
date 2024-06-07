using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class PageableResultDtoDataChangeResponseExample : IExamplesProvider<PageableResultDto<DataChangeDto>>
  {
    public PageableResultDto<DataChangeDto> GetExamples()
    {
      var result = new PageableResultDto<DataChangeDto>()
      {
        Items = new DataChangeDto[]
        {
          new DataChangeDto()
          {
            Id = "d8e2d6d5a65e4ad09967bd8ae50c2eea",
            ObjectId = "a5f054562a034e3d916c450fae09877b",
            ObjectType = "Team",
            OperationTime = DateTime.Parse("2023-10-16,23:20:00"),
            OperationType = "Dodanie użytkownika do zespołu",
            OperationResult = true,
            ChangeDetails = new List<ChangeDetailsDto>
            {
              new ChangeDetailsDto
              {
                PropertyName = "UserIds",
                NewValue = "82v490c15e094e7aca11f06Ee14b69c9"
              }
            }
          },
          new DataChangeDto
          {
            Id = "d8e2d6d5a65e4ad09967bd8ae50c2eea",
            ObjectId = "a5f054562a034e3d916c450fae09877b",
            ObjectType = "Team",
            OperationTime = DateTime.Parse("2023-10-16,23:20:00"),
            OperationType = "Zmiana nazwy zespołu",
            OperationResult = true,
            ChangeDetails = new List<ChangeDetailsDto>
            {
              new ChangeDetailsDto
              {
                PropertyName = "Name",
                OldValue = "Java",
                NewValue = "Kotlin"
              }
            }
          }
        },
        Pagination = new PaginationDto { PageNumber = 0, PageSize = 50, TotalElements = 2 },
      };
      return result;
    }
  }
}
