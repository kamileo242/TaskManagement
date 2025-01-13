using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TaskManagement.Integration
{
  [TestFixture]
  public abstract class BaseTest
  {
    protected TaskManagementClient client;
    protected IHost host;
    protected HttpClient httpClient = new HttpClient();
    protected DateTime startTestTime;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
      DatabaseHelper.InitializeDatabase();
      client = new TaskManagementClient("http://localhost:5000", httpClient);

      host = Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStartup<WebApiHost.Startup>();
          webBuilder.UseUrls("http://*:5000");
        })
        .Build();

      await host.StartAsync();
    }

    [SetUp]
    public async Task Setup()
    {
      startTestTime = DateTime.Now;
      await DatabaseHelper.ClearAllCollection();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
      await host.StopAsync();
      host.Dispose();

      httpClient.Dispose();
    }
  }
}
