using Models;

namespace DataLayer
{
  /// <summary>
  /// Zbiór operacji wykonywanych na repozytorium historii zmian
  /// </summary>
  public interface IDataChangeRepository : IRepository<DataChange, Guid>
  {
  }
}
