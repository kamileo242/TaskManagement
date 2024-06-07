using DataLayer.Dbos;
using Models;

namespace DataLayer.Repositories
{
  public class UserRepository : AbstractRepository<User, UserDbo, Guid>, IUserRepository
  {
    public UserRepository(DatabaseSetup setup, IDboConverter dboConverter) : base(setup, dboConverter, DatabaseCollectionName.User)
    {
    }
  }
}
