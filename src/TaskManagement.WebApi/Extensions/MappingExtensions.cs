using Models;
using WebApi.Dtos;
using Task = Models.Task;

namespace WebApi.Extensions
{
  public static class MappingExtensions
  {
    public static Team ToModel(this StoreTeamDto dto)
    => new()
    {
      Name = dto.Name,
    };

    public static Change<Team> ToModel(this ChangeDto<PatchTeamDto> dto)
    => new()
    {
      Data = new()
      {
        Name = dto.Data.Name,
      },
      Updates = dto.Updates,
    };

    public static User ToModel(this StoreUserDto dto)
    => new()
    {
      Name = dto.Name,
      Surname = dto.Surname,
      Email = dto.Email,
      PhoneNumber = dto.PhoneNumber,
      Position = dto.Position,
    };

    public static Change<User> ToModel(this ChangeDto<PatchUserDto> dto)
    => new()
    {
      Data = new()
      {
        Name = dto.Data.Name,
        Surname = dto.Data.Surname,
        Email = dto.Data.Email,
        PhoneNumber = dto.Data.PhoneNumber,
        Position = dto.Data.Position,
      },
      Updates = dto.Updates,
    };

    public static Task ToModel(this StoreTaskDto dto)
    => new()
    {
      Title = dto.Title,
      Deadline = dto.Deadline.GetValueOrDefault(),
      Priority = dto.Priority,
      Description = dto.Description,
    };

    public static Change<Task> ToModel(this ChangeDto<PatchTaskDto> dto)
    => new()
    {
      Data = new()
      {
        Title = dto.Data.Title,
        Deadline = dto.Data.Deadline.GetValueOrDefault(),
        Priority = dto.Data.Priority,
        Description = dto.Data.Description,
      },
      Updates = dto.Updates,

    };

    public static Project ToModel(this StoreProjectDto dto)
    => new()
    {
      Title = dto.Title,
      Deadline = dto.Deadline.GetValueOrDefault(),
      Description = dto.Description,
    };

    public static Change<Project> ToModel(this ChangeDto<PatchProjectDto> dto)
    => new()
    {
      Data = new()
      {
        Title = dto.Data.Title,
        Deadline = dto.Data.Deadline.GetValueOrDefault(),
        Description = dto.Data.Description,
      },
      Updates = dto.Updates,
    };

    public static Comment ToModel(this StoreCommentDto dto)
    => new()
    {
      Author = dto.Author,
      Content = dto.Content,
    };
  }
}
