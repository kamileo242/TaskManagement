using FluentAssertions;

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
  }
}
