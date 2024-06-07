using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class DataChangeDtoResponseExample : IExamplesProvider<DataChangeDto>
  {
    public DataChangeDto GetExamples()
      => new()
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
      };
  }
}
