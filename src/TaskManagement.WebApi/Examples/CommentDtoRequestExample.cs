using Swashbuckle.AspNetCore.Filters;
using WebApi.Dtos;

namespace WebApi.Examples
{
  public class CommentDtoRequestExample : IExamplesProvider<StoreCommentDto>
  {
    public StoreCommentDto GetExamples()
      => new()
      {
        Author = "Mariusz Kowalski",
        Content = "Należy uwzględnić listę, która będzie zawierać właściwości, które uległy zmianie",
      };
  }
}
