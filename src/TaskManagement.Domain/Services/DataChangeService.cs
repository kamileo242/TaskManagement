using DataLayer;
using Models;

namespace Domain.Services
{
  public class DataChangeService : IDataChangeService
  {
    private readonly IDataChangeRepository repository;

    public DataChangeService(IDataChangeRepository repository)
    {
      this.repository = repository;
    }

    public async Task<DataChange> GetByIdAsync(Guid id)
    => await repository.GetByIdAsync(id);

    public async Task<PageableResult<DataChange>> GetAllAsync(PageableInput input)
      => await repository.GetAllAsync(input);
  }
}
