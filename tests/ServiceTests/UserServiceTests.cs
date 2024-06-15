using DataLayer;
using Domain;
using Domain.Services;
using FluentAssertions;
using Models;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace ServiceTests
{
  [TestFixture]
  public class UserServiceTests
  {
    private Mock<IUserRepository> mockRepository;
    private Mock<IHistoryUpdater> mockUpdater;
    private IUserService userService;

    [SetUp]
    public void Setup()
    {
      mockRepository = new Mock<IUserRepository>();
      mockUpdater = new Mock<IHistoryUpdater>();
      userService = new UserService(mockRepository.Object);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_user_when_valid_id()
    {
      var expected = new User
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "Johnny",
        Surname = "Deep",
        Position = "Actor",
        Email = "johnny.deep@example.com",
        PhoneNumber = "123456789"
      };
      mockRepository.Setup(s => s.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

      var result = await userService.GetByIdAsync(expected.Id);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_null_when_user_not_found()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      mockRepository.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User) null);

      var result = await userService.GetByIdAsync(userId);

      result.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_Should_return_users_with_pagination()
    {
      var input = new PageableInput
      {
        PageNumber = 0,
        PageSize = 50,
      };
      var expected = new PageableResult<User>
      {
        Items = new User[]
        {
          new User
          {
            Id = Guid.Parse("00000000000000000000000000000001"),
            Name = "Johnny",
            Surname = "Deep",
            Position = "Actor",
            Email = "johnny.deep@example.com",
            PhoneNumber = "123456789"
          }
        },
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 1
        }
      };
      mockRepository.Setup(s => s.GetAllAsync(input)).ReturnsAsync(expected);

      var result = await userService.GetAllAsync(input);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddAsync_Should_return_stored_user()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var userToAdd = new User
      {
        Name = "Johnny",
        Surname = "Deep",
        Position = "Actor",
        Email = "johnny.deep@example.com",
        PhoneNumber = "123456789"
      };
      mockRepository.Setup(s => s.StoreAsync(It.IsAny<User>()))
                    .ReturnsAsync((User user) =>
                    {
                      user.Id = userId;
                      return user;
                    });

      var result = await userService.AddAsync(mockUpdater.Object, userToAdd);

      result.Should().NotBeNull();
      result.Id.Should().Be("00000000000000000000000000000001");
      result.Should().BeEquivalentTo(userToAdd, options => options.Excluding(u => u.Id));
      result.Id.Should().Be(userId);
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_user_data_has_not_name()
    {
      var invalidUser = new User()
      {
        Surname = "Deep",
        Position = "Actor",
        Email = "johnny.deep@example.com",
        PhoneNumber = "123456789"
      };

      var action = async () => await userService.AddAsync(mockUpdater.Object, invalidUser);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Pozycja 'Imię' jest wymagana.");
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_user_data_has_not_surname()
    {
      var invalidUser = new User()
      {
        Name = "Johnny",
        Position = "Actor",
        Email = "johnny.deep@example.com",
        PhoneNumber = "123456789"
      };

      var action = async () => await userService.AddAsync(mockUpdater.Object, invalidUser);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Pozycja 'Nazwisko' jest wymagana.");
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_user_data_has_not_position()
    {
      var invalidUser = new User()
      {
        Name = "Johnny",
        Surname = "Deep",
        Email = "johnny.deep@example.com",
        PhoneNumber = "123456789"
      };

      var action = async () => await userService.AddAsync(mockUpdater.Object, invalidUser);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Pozycja 'Stanowisko' jest wymagana.");
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_user_data_has_invalid_email()
    {
      var invalidUser = new User()
      {
        Name = "Johnny",
        Surname = "Deep",
        Position = "Actor",
        PhoneNumber = "123456789",
        Email = "niewlasciwy"
      };

      var action = async () => await userService.AddAsync(mockUpdater.Object, invalidUser);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Pozycja 'Email' musi zawierać znak '@' oraz domenę końcową.");
    }

    [Test]
    public async Task AddAsync_Should_Throw_exception_when_user_data_has_not_phone_number()
    {
      var invalidUser = new User()
      {
        Name = "Johnny",
        Surname = "Deep",
        Position = "Actor",
        Email = "johnny.deep@example.com",
        PhoneNumber = "12345678a"
      };

      var action = async () => await userService.AddAsync(mockUpdater.Object, invalidUser);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Pozycja 'Numer telefonu' musi mieć dokładnie 9 znaków i zawierać tylko cyfry.");
    }

    [Test]
    public async Task DeleteAsync_Should_remove_user()
    {
      var userIdToDelete = Guid.Parse("00000000000000000000000000000001");

      await userService.DeleteAsync(mockUpdater.Object, userIdToDelete);

      mockRepository.Verify(s => s.RemoveAsync(userIdToDelete), Times.Once);
    }

    [Test]
    public async Task Patch_Should_return_null_when_userId_not_found()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var changes = new Change<User>() { Data = new User { Name = "Jack" }, Updates = new List<string> { nameof(User.Name) } };

      var result = await userService.PatchAsync(mockUpdater.Object, userId, changes);

      result.Should().BeNull();
    }

    [Test]
    public async Task Patch_Should_throw_exception_when_email_is_invalid()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var changes = new Change<User>() { Data = new User { Email = "invalid" }, Updates = new List<string> { nameof(User.Email) } };

      var action = async () => await userService.PatchAsync(mockUpdater.Object, userId, changes);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Pozycja 'Email' musi zawierać znak '@' oraz domenę końcową.");
    }

    [Test]
    public async Task Patch_Should_throw_exception_when_phone_number_is_invalid()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var changes = new Change<User>() { Data = new User { PhoneNumber = "123456" }, Updates = new List<string> { nameof(User.PhoneNumber) } };

      var action = async () => await userService.PatchAsync(mockUpdater.Object, userId, changes);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Pozycja 'Numer telefonu' musi mieć dokładnie 9 znaków i zawierać tylko cyfry.");
    }

    [Test]
    public async Task Patch_Should_return_modified_object_when_valid_data()
    {
      var userId = Guid.Parse("00000000000000000000000000000001");
      var changes = new Change<User>() { Data = new User { Name = "Jack" }, Updates = new List<string> { nameof(User.Name) } };
      var expected = new User
      {
        Id = userId,
        Name = "Johnny",
        Surname = "Deep",
        Position = "Actor",
        Email = "johnny.deep@example.com",
        PhoneNumber = "123456789"
      };
      mockRepository.Setup(s => s.GetByIdAsync(expected.Id)).ReturnsAsync(expected);
      mockRepository.Setup(s => s.ChangeOneAsync(userId, changes)).ReturnsAsync(expected);

      var result = await userService.PatchAsync(mockUpdater.Object, userId, changes);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }
  }
}
