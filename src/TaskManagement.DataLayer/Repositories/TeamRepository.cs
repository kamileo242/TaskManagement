using DataLayer.Dbos;
using Models;

namespace DataLayer.Repositories
{
  public class TeamRepository : AbstractRepository<Team, TeamDbo, Guid>, ITeamRepository
  {
    public TeamRepository(DatabaseSetup setup, IDboConverter dboConverter) : base(setup, dboConverter, DatabaseCollectionName.Team)
    {
    }
  }
}
