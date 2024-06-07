using DataLayer.Dbos;
using Models;
using Task = Models.Task;

namespace DataLayer.Repositories
{
  public class TaskRepository : AbstractRepository<Task, TaskDbo, Guid>, ITaskRepository
  {
    public TaskRepository(DatabaseSetup setup, IDboConverter dboConverter) : base(setup, dboConverter, DatabaseCollectionName.Task)
    {
    }
  }
}
