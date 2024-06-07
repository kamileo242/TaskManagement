using Models;

namespace DataLayer
{
  /// <summary>
  /// Zbiór operacji wykonywanych na repozytorium zespołu
  /// </summary>
  public interface ITeamRepository : IRepository<Team, Guid>
  {
  }
}
