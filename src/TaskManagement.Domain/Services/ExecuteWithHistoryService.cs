using DataLayer;
using Models;
using TimeProvider = Domain.Providers.TimeProvider;

namespace Domain.Services
{
  public class ExecuteWithHistoryService : IExecuteWithHistoryService
  {
    private readonly IDataChangeRepository dataChangeRepository;

    public ExecuteWithHistoryService(IDataChangeRepository dataChangeRepository)
    {
      this.dataChangeRepository = dataChangeRepository;
    }

    public async Task<TResult> Execute<TResult>(
        Func<IHistoryUpdater, Task<TResult>> action,
        OperationContext operationContext
    ) where TResult : class
        => await ExecuteInternal(action, operationContext);

    private async Task<TResult> ExecuteInternal<TResult>(
        Func<IHistoryUpdater, Task<TResult>> action,
        OperationContext operationContext
    ) where TResult : class
    {
      TResult result = null;

      var dataChange = new DataChange()
      {
        OperationType = operationContext.OperationType,
        OperationTime = TimeProvider.GetTime()
      };

      var historyUpdater = new HistoryUpdater(dataChange);
      try
      {
        result = await action.Invoke(historyUpdater);

        dataChange.ObjectType = historyUpdater.ObjectType;
        dataChange.ObjectId = historyUpdater.ObjectId;
        dataChange.ChangeDetails = historyUpdater.ChangeDetails;
        dataChange.OperationResult = true;
      }
      catch (Exception ex)
      {
        dataChange.OperationResult = false;

        throw;
      }
      finally
      {
        await dataChangeRepository.StoreAsync(dataChange);
      }
      return result;
    }
  }
}
