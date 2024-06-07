using Models;
using WebApi.Dtos;

namespace WebApi
{
  /// <summary>
  /// Zbiór operacji do konwersji modeli na obiekty Dto
  /// </summary>
  public interface IDtoBuilder
  {
    /// <summary>
    /// Konwersja modelu zespołu na obiekt Dto
    /// </summary>
    /// <param name="model">Model zespołu</param>
    /// <returns>TeamDto</returns>
    TeamDto ConvertToTeamDto(Team model);

    /// <summary>
    /// Konwersja modelu użytkownika na obiekt Dto
    /// </summary>
    /// <param name="model">Model użytkownika</param>
    /// <returns>UserDto</returns>
    UserDto ConvertToUserDto(User model);

    /// <summary>
    /// Konwersja modelu zadania na obiekt Dto
    /// </summary>
    /// <param name="model">Model zadania</param>
    /// <returns>TaskDto</returns>
    TaskDto ConvertToTaskDto(Models.Task model);

    /// <summary>
    /// Konwersja modelu projektu na obiekt Dto
    /// </summary>
    /// <param name="model">Model projektu</param>
    /// <returns>ProjectDto</returns>
    ProjectDto ConvertToProjectDto(Project model);

    /// <summary>
    /// Konwersja modelu komentarza na obiekt Dto
    /// </summary>
    /// <param name="model">Model komentarza</param>
    /// <returns>CommentDto</returns>
    CommentDto ConvertToCommentDto(Comment model);

    /// <summary>
    /// Konwersja modelu historii zmian na obiekt Dto
    /// </summary>
    /// <param name="model">Model historii zmian</param>
    /// <returns>DataChangeDto</returns>
    DataChangeDto ConvertToDataChangeDto(DataChange model);

    /// <summary>
    /// Konwersja listy historii zmian na obiekt Dto
    /// </summary>
    /// <param name="model">Lista historii zmian</param>
    /// <returns>ChangeDtoDataChangeDto></returns>
    PageableResultDto<DataChangeDto> ConvertToPageableResultDtoDataChangeDto(PageableResult<DataChange> model);

    /// <summary>
    /// Konwersja modelu zespołu na obiekt Dto
    /// </summary>
    /// <param name="model">Model zespołu</param>
    /// <returns>TeamDto</returns>
    PageableResultDto<TeamDto> ConvertToPageableResultDtoTeamDto(PageableResult<Team> model);

    /// <summary>
    /// Konwersja modelu użytkownika na obiekt Dto
    /// </summary>
    /// <param name="model">Model użytkownika</param>
    /// <returns>UserDto</returns>
    PageableResultDto<UserDto> ConvertToPageableResultDtoUserDto(PageableResult<User> model);

    /// <summary>
    /// Konwersja modelu zadania na obiekt Dto
    /// </summary>
    /// <param name="model">Model zadania</param>
    /// <returns>TaskDto</returns>
    PageableResultDto<TaskDto> ConvertToPageableResultDtoTaskDto(PageableResult<Models.Task> model);

    /// <summary>
    /// Konwersja modelu projektu na obiekt Dto
    /// </summary>
    /// <param name="model">Model projektu</param>
    /// <returns>ProjectDto</returns>
    PageableResultDto<ProjectDto> ConvertToPageableResultDtoProjectDto(PageableResult<Project> model);
  }
}
