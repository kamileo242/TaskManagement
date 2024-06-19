using Swashbuckle.AspNetCore.Filters;
using TaskManagement.WebApi.Dtos;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class TaskDtoResponseExample : IExamplesProvider<TaskDto>
  {
    public TaskDto GetExamples()
      => new()
      {
        Id = "d8e2d6d5a65e4ad09967bd8ae50c2eea",
        Title = "Implementacja historii zmian",
        Description = "Zaimplementowanie historii zmian zapisującej operacje wykonywane na wszytskich ewidencjach",
        Priority = new TaskPriorityDto { Id = "normalny", Name = "Normalny" },
        Deadline = DateTime.Parse("2024-04-01"),
        Status = Models.Statueses.TaskStatus.NotStarted.Value,
        CreatedAt = DateTime.Parse("2024-02-19"),
        Comments = new List<CommentDto>
        {
          new CommentDto()
          {
            Id = "82v490c15e094e7aca11f06Ee14b69c9",
            Author = "Mariusz Kowalski",
            Content = "Należy uwzględnić listę, która będzie zawierać właściwości, które uległy zmianie",
            CreatedAt = DateTime.Parse("2024-02-19"),
          }
        }
      };
  }
}
