using Models;

namespace DataLayer
{
  /// <summary>
  /// Zbiór operacji wykonywanych na repozytorium użytkownika
  /// </summary>
  public interface IUserRepository : IRepository<User, Guid>
  {
  }
}
