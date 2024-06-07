namespace DataLayer
{
  /// <summary>
  /// Zbiór operacji wykonywanych na repozytorium zadania
  /// </summary>
  public interface ITaskRepository : IRepository<Models.Task, Guid>
  {
  }
}
