using Models;

namespace DataLayer
{
  /// <summary>
  /// Zbiór operacji wykonywanych na repozytorium projektu
  /// </summary>
  public interface IProjectRepository : IRepository<Project, Guid>
  {
  }
}
