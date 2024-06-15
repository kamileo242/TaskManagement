using Models;

namespace Domain
{
  /// <summary>
  /// Serwis akcji wraz z zapisem historii zmian
  /// </summary>
  public interface IExecuteWithHistoryService
  {
    /// <summary>
    /// Wykonanie akcji wraz zapisem historii zmian.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="action">Akcja do wykonania.</param>
    /// <param name="operationContext">Kontekst wykonywanej operacji.</param>
    /// <returns></returns>
    Task<TResult> Execute<TResult>(Func<IHistoryUpdater, Task<TResult>> action, OperationContext operationContext) where TResult : class;
  }
}
