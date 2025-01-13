using DataLayer;
using Models;
using TaskManagement.Models.Exceptions;

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
    {
      var history = await repository.GetByIdAsync(id);

      if (history == null)
      {
        throw new MissingDataException($"Nie znaleziono historii zmian o id {id}");
      }

      return history;
    }

    public async Task<PageableResult<DataChange>> GetAllAsync(PageableInput input)
      => await repository.GetAllAsync(input);
  }
}
