using DataLayer;
using Domain.Providers;
using Models;
using TaskManagement.Models.Exceptions;
using Task = System.Threading.Tasks.Task;

namespace Domain.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository repository;

    public UserService(IUserRepository repository)
    {
      this.repository = repository;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
      var user = await repository.GetByIdAsync(id);

      if (user == null)
      {
        throw new MissingDataException($"Nie znaleziono użytkownika o id: {id}.");
      }

      return user;
    }

    public User GetById(Guid id)
    {
      var user = repository.GetByIdAsync(id).GetAwaiter().GetResult();

      if (user == null)
      {
        throw new MissingDataException($"Nie znaleziono użytkownika o id: {id}.");
      }

      return user;
    }

    public async Task<PageableResult<User>> GetAllAsync(PageableInput input)
      => await repository.GetAllAsync(input);

    public async Task<User> AddAsync(IHistoryUpdater updater, User user)
    {
      ValidateStoreUser(user);

      user.Id = GuidProvider.GenetareGuid();

      var result = await repository.StoreAsync(user);

      updater.SetObjectId<User>(result.Id);
      updater.SetChangeDetails(null, result);

      return result;
    }

    public async Task DeleteAsync(IHistoryUpdater updater, Guid id)
    {
      updater.SetObjectId<User>(id);

      await repository.RemoveAsync(id);
    }

    public async Task<User> PatchAsync(IHistoryUpdater updater, Guid id, Change<User> user)
    {
      ValidatePatchUser(user);

      var existingUser = await repository.GetByIdAsync(id);
      if (existingUser == null)
      {
        throw new MissingDataException($"Nie znaleziono użytkownika o id: {id}.");
      }

      var result = await repository.ChangeOneAsync(id, user);

      updater.SetObjectId<User>(result.Id);
      updater.SetChangeDetails(existingUser, result);

      return result;
    }

    private void ValidateStoreUser(User user)
    {
      if (string.IsNullOrWhiteSpace(user?.Name))
      {
        throw new IncorrectDataException("Pozycja 'Imię' jest wymagana.");
      }

      if (string.IsNullOrWhiteSpace(user?.Surname))
      {
        throw new IncorrectDataException("Pozycja 'Nazwisko' jest wymagana.");
      }

      if (string.IsNullOrWhiteSpace(user?.Position))
      {
        throw new IncorrectDataException("Pozycja 'Stanowisko' jest wymagana.");
      }

      if (!user.Email.Contains("@") && (!user.Email.Contains(".pl") || !user.Email.Contains(".com")))
      {
        throw new IncorrectDataException("Pozycja 'Email' musi zawierać znak '@' oraz domenę końcową.");
      }

      if (user.PhoneNumber.Length != 9 || !user.PhoneNumber.All(char.IsDigit))
      {
        throw new IncorrectDataException("Pozycja 'Numer telefonu' musi mieć dokładnie 9 znaków i zawierać tylko cyfry.");
      }
    }

    private void ValidatePatchUser(Change<User> user)
    {
      if (user.Updates.Contains(nameof(User.Email)))
      {
        if (!user.Data.Email.Contains("@") && (!user.Data.Email.Contains(".pl") || !user.Data.Email.Contains(".com")))
        {
          throw new IncorrectDataException("Pozycja 'Email' musi zawierać znak '@' oraz domenę końcową.");
        }
      }

      if (user.Updates.Contains(nameof(User.PhoneNumber)))
      {
        if (user.Data.PhoneNumber.Length != 9 || !user.Data.PhoneNumber.All(char.IsDigit))
        {
          throw new IncorrectDataException("Pozycja 'Numer telefonu' musi mieć dokładnie 9 znaków i zawierać tylko cyfry.");
        }
      }
    }
  }
}
