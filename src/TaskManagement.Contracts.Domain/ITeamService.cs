using Models;
using Task = System.Threading.Tasks.Task;

namespace Domain
{
  /// <summary>
  /// Zbiór operacji wykonywanych na serwisie zespołu
  /// </summary>
  public interface ITeamService
  {
    /// <summary>
    /// Pobierz zespół po id
    /// </summary>
    /// <param name="id">Identyfikator zespołu</param>
    /// <returns>Zespół</returns>
    Task<Team> GetByIdAsync(Guid id);

    /// <summary>
    /// Pobierz wszystkie zespoły
    /// </summary>
    /// <returns>Lista zespołów</returns>
    Task<PageableResult<Team>> GetAllAsync(PageableInput input);

    /// <summary>
    /// Dodaj nowy zespół
    /// </summary>
    /// <param name="team">Zespół</param>
    /// <returns>Nowy zespół</returns>
    Task<Team> AddAsync(Team team);

    /// <summary>
    /// Usuń zespół
    /// </summary>
    /// <param name="id">Identyfikator zespołu</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Dodaj pracownika do zespołu
    /// </summary>
    /// <param name="teamId">Identyfikator zespołu</param>
    /// <param name="userId">Identyfikator pracownika</param>
    /// <returns>Zespół z dodanym pracownikiem</returns>
    Task<Team> AddUserToTeam(Guid teamId, Guid userId);

    /// <summary>
    /// Ustal leadera zespołu
    /// </summary>
    /// <param name="teamId">Identyfikator zespołu</param>
    /// <param name="userId">Identyfikator leadera zespołu</param>
    /// <returns>Zespół z leaderem</returns>
    Task<Team> AddTeamLeader(Guid teamId, Guid userId);

    /// <summary>
    /// Usuń pracownika z zespołu
    /// </summary>
    /// <param name="teamId">Identyfikator zespołu</param>
    /// <param name="userId">Identyfikator pracownika</param>
    /// <returns>Zespół z usuniętym pracownikiem</returns>
    Task<Team> DeleteUserFromTeam(Guid teamId, Guid userId);

    /// <summary>
    /// Zmień wybrane właściwości zespołu
    /// </summary>
    /// <param name="id">Identyfikator zespołu</param>
    /// <param name="team">Dane zespołu do modyfikacji</param>
    /// <returns>Zmodyfikowany zespół</returns>
    Task<Team> Patch(Guid id, Change<Team> team);
  }
}
