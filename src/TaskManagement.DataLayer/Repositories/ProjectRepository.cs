using DataLayer.Dbos;
using Models;

namespace DataLayer.Repositories
{
  public class ProjectRepository : AbstractRepository<Project, ProjectDbo, Guid>, IProjectRepository
  {
    public ProjectRepository(DatabaseSetup setup, IDboConverter dboConverter) : base(setup, dboConverter, DatabaseCollectionName.Project)
    {
    }
  }
}
