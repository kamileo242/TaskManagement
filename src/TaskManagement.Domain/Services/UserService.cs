using DataLayer;
using Domain.Providers;
using Models;
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
      => await repository.GetByIdAsync(id);

    public User GetById(Guid id)
    {
      var user = repository.GetByIdAsync(id).GetAwaiter().GetResult();

      if (user == null)
      {
        return null;
      }

      return user;
    }

    public async Task<PageableResult<User>> GetAllAsync(PageableInput input)
      => await repository.GetAllAsync(input);

    public async Task<User> AddAsync(User user)
    {
      ValidateStoreUser(user);

      user.Id = GuidProvider.GenetareGuid();

      return await repository.StoreAsync(user);
    }

    public async Task DeleteAsync(Guid id)
      => await repository.RemoveAsync(id);

    public async Task<User> Patch(Guid id, Change<User> user)
    {
      ValidatePatchUser(user.Data);

      return await repository.ChangeOneAsync(id, user);
    }

    private void ValidateStoreUser(User user)
    {
      if (user == null)
      {
        throw new InvalidDataException("Nie podano żadnych danych !");
      }

      if (string.IsNullOrWhiteSpace(user?.Name))
      {
        throw new InvalidDataException("Pozycja 'Imię' jest wymagana !");
      }

      if (string.IsNullOrWhiteSpace(user?.Surname))
      {
        throw new InvalidDataException("Pozycja 'Nazwisko' jest wymagana !");
      }

      if (string.IsNullOrWhiteSpace(user?.Position))
      {
        throw new InvalidDataException("Pozycja 'Stanowisko' jest wymagana !");
      }

      if (!user.Email.Contains("@") && (!user.Email.Contains(".pl") || !user.Email.Contains(".com")))
      {
        throw new InvalidDataException("Pozycja 'Email' musi zawierać znak '@' oraz domenę końcową!");
      }

      if (string.IsNullOrWhiteSpace(user?.PhoneNumber))
      {
        throw new InvalidDataException("Pozycja 'Numer telefonu' jest wymagana!");
      }

      if (user.PhoneNumber.Length != 9 || !user.PhoneNumber.All(char.IsDigit))
      {
        throw new InvalidDataException("Pozycja 'Numer telefonu' musi mieć dokładnie 9 znaków i zawierać tylko cyfry!");
      }
    }

    private void ValidatePatchUser(User user)
    {
      if (user.Email != null)
      {
        if (!user.Email.Contains("@") && (!user.Email.Contains(".pl") || !user.Email.Contains(".com")))
        {
          throw new InvalidDataException("Pozycja 'Email' musi zawierać znak '@' oraz domenę końcową!");
        }
      }

      if (user.PhoneNumber != null)
      {
        if (user.PhoneNumber.Length != 9 || !user.PhoneNumber.All(char.IsDigit))
        {
          throw new InvalidDataException("Pozycja 'Numer telefonu' musi mieć dokładnie 9 znaków i zawierać tylko cyfry!");
        }
      }
    }
  }
}
