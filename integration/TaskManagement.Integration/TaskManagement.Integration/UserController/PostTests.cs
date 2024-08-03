using FluentAssertions;
using MongoDB.Bson;

namespace TaskManagement.Integration.UserController
{
  [TestFixture]
  public class PostTests : BaseTest
  {
    [Test]
    public async Task Post_Should_store_user_when_valid_data()
    {
      var storeUser = new StoreUserDto
      {
        Name = "Tomasz",
        Surname = "Kowalski",
        Email = "tkowalski@gmail.com",
        PhoneNumber = "123456789",
        Position = "Praktykant"
      };
      var expected = new UserDto
      {
        Name = "Tomasz",
        Surname = "Kowalski",
        Email = "tkowalski@gmail.com",
        PhoneNumber = "123456789",
        Position = "Praktykant"
      };

      var result = await client.UserPOSTAsync(storeUser);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected, options => options.Excluding(s => s.Id));
      var resultInDataBase = DatabaseHelper.GetElementFromCollection("users");
      resultInDataBase.Should().NotBeNull();
      resultInDataBase["_id"].Should().NotBeNull();
      resultInDataBase["name"].Should().Be(expected.Name);
      resultInDataBase["surname"].Should().Be(expected.Surname);
      resultInDataBase["email"].Should().Be(expected.Email);
      resultInDataBase["position"].Should().Be(expected.Position);
      resultInDataBase["phone_number"].Should().Be(expected.PhoneNumber);
    }

    [Test]
    public async Task Post_Should_throw_exception_when_missing_name()
    {
      var storeUser = new StoreUserDto
      {
        Surname = "Kowalski",
        Email = "tkowalski@gmail.com",
        PhoneNumber = "123456789",
        Position = "Praktykant"
      };

      var action = async () => await client.UserPOSTAsync(storeUser);

      var exception = await action.Should().ThrowAsync<ApiException>();
      exception.Which.StatusCode.Should().Be(400);
      exception.WithMessage("*Błąd po stronie użytkownika, błędne dane wejściowe do usługi.*");
      var resultInDataBase = DatabaseHelper.GetElementFromCollection("users");
      resultInDataBase.Should().BeNull();
    }

    [Test]
    public async Task Post_Should_throw_exception_when_missing_surame()
    {
      var storeUser = new StoreUserDto
      {
        Name = "Tomasz",
        Email = "tkowalski@gmail.com",
        PhoneNumber = "123456789",
        Position = "Praktykant"
      };

      var action = async () => await client.UserPOSTAsync(storeUser);

      var exception = await action.Should().ThrowAsync<ApiException>();
      exception.Which.StatusCode.Should().Be(400);
      exception.WithMessage("*Błąd po stronie użytkownika, błędne dane wejściowe do usługi.*");
      var resultInDataBase = DatabaseHelper.GetElementFromCollection("users");
      resultInDataBase.Should().BeNull();
    }

    [Test]
    public async Task Post_Should_throw_exception_when_missing_position()
    {
      var storeUser = new StoreUserDto
      {
        Name = "Tomasz",
        Email = "tkowalski@gmail.com",
        PhoneNumber = "123456789",
        Surname = "Kowalski",
      };

      var action = async () => await client.UserPOSTAsync(storeUser);

      var exception = await action.Should().ThrowAsync<ApiException>();
      exception.Which.StatusCode.Should().Be(400);
      exception.WithMessage("*Błąd po stronie użytkownika, błędne dane wejściowe do usługi.*");
      var resultInDataBase = DatabaseHelper.GetElementFromCollection("users");
      resultInDataBase.Should().BeNull();
    }

    [Test]
    public async Task Post_Should_throw_exception_when_number_is_too_short()
    {
      var storeUser = new StoreUserDto
      {
        Name = "Tomasz",
        Email = "tkowalski@gmail.com",
        PhoneNumber = "12345678",
        Surname = "Kowalski",
        Position = "Praktykant"
      };

      var action = async () => await client.UserPOSTAsync(storeUser);

      var exception = await action.Should().ThrowAsync<ApiException>();
      exception.Which.StatusCode.Should().Be(400);
      exception.WithMessage("*Błąd po stronie użytkownika, błędne dane wejściowe do usługi.*");
      var resultInDataBase = DatabaseHelper.GetElementFromCollection("users");
      resultInDataBase.Should().BeNull();
    }

    [Test]
    public async Task Post_Should_throw_exception_when_number_is_invalid()
    {
      var storeUser = new StoreUserDto
      {
        Name = "Tomasz",
        Email = "tkowalski@gmail.com",
        PhoneNumber = "12345678a",
        Surname = "Kowalski",
        Position = "Praktykant"
      };

      var action = async () => await client.UserPOSTAsync(storeUser);

      var exception = await action.Should().ThrowAsync<ApiException>();
      exception.Which.StatusCode.Should().Be(400);
      exception.WithMessage("*Błąd po stronie użytkownika, błędne dane wejściowe do usługi.*");
      var resultInDataBase = DatabaseHelper.GetElementFromCollection("users");
      resultInDataBase.Should().BeNull();
    }

    [Test]
    public async Task Post_Should_throw_exception_when_invalid_email()
    {
      var storeUser = new StoreUserDto
      {
        Name = "Tomasz",
        Email = "tkowalskigmail.com",
        PhoneNumber = "123456789",
        Surname = "Kowalski",
        Position = "Praktykant"
      };

      var action = async () => await client.UserPOSTAsync(storeUser);

      var exception = await action.Should().ThrowAsync<ApiException>();
      exception.Which.StatusCode.Should().Be(400);
      exception.WithMessage("*Błąd po stronie użytkownika, błędne dane wejściowe do usługi.*");
      var resultInDataBase = DatabaseHelper.GetElementFromCollection("users");
      resultInDataBase.Should().BeNull();
    }

    [Test]
    public async Task Post_Should_store_activity_when_operation_was_successfull()
    {
      var storeUser = new StoreUserDto
      {
        Name = "Tomasz",
        Email = "tkowalski@gmail.com",
        PhoneNumber = "123456789",
        Surname = "Kowalski",
        Position = "Praktykant"
      };

      var result = await client.UserPOSTAsync(storeUser);

      var endTestTime = DateTime.Now;
      result.Should().NotBeNull();
      var resultInDataBase = DatabaseHelper.GetElementFromCollection("data_change");
      resultInDataBase.Should().NotBeNull();
      resultInDataBase["_id"].Should().NotBeNull();
      resultInDataBase["operation_time"].ToLocalTime().Should().BeAfter(startTestTime);
      resultInDataBase["operation_time"].ToLocalTime().Should().BeBefore(endTestTime);
      resultInDataBase["object_type"].Should().Be("User");
      resultInDataBase["object_id"].Should().Be(result.Id);
      resultInDataBase["operation_result"].Should().Be(true);
      BsonArray changes = resultInDataBase["change_details"].AsBsonArray;
      changes.Should().HaveCount(6);
      changes[0]["property_name"].Should().Be("Id");
      changes[0]["new_value"].Should().NotBeNull();

      changes[1]["property_name"].Should().Be("Name");
      changes[1]["new_value"].Should().Be(storeUser.Name);

      changes[2]["property_name"].Should().Be("Surname");
      changes[2]["new_value"].Should().Be(storeUser.Surname);

      changes[3]["property_name"].Should().Be("Email");
      changes[3]["new_value"].Should().Be(storeUser.Email);

      changes[4]["property_name"].Should().Be("Position");
      changes[4]["new_value"].Should().Be(storeUser.Position);

      changes[5]["property_name"].Should().Be("PhoneNumber");
      changes[5]["new_value"].Should().Be(storeUser.PhoneNumber);
    }
  }
}
