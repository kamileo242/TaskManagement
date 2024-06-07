using DataLayer.Dbos;
using Models;

namespace DataLayer.Repositories
{
  public class DataChangeRepository : AbstractRepository<DataChange, DataChangeDbo, Guid>, IDataChangeRepository
  {
    public DataChangeRepository(DatabaseSetup setup, IDboConverter dboConverter) : base(setup, dboConverter, DatabaseCollectionName.DataChange)
    {
    }
  }
}
