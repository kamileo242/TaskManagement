using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models;
using System.Reflection;
using Task = System.Threading.Tasks.Task;

namespace Bootstrap
{
  internal class Startup
  {
    static async Task Main(string[] args)
    {
      try
      {
        var host = Host.CreateDefaultBuilder(args)
          .UseServiceProviderFactory(new AutofacServiceProviderFactory())
          .ConfigureContainer<ContainerBuilder>((context, builder) =>
          {
            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            builder.RegisterInstance(context.Configuration.GetSection(DatabaseSetup.SectionName).Get<DatabaseSetup>());
          })
          .ConfigureAppConfiguration(builder =>
          {
            builder.AddJsonFile("appsettings.json", optional: false);
          })
          .ConfigureWebHostDefaults(webBuilder =>
          {
            webBuilder.UseStartup<WebApi.Startup>();
          })
          .Build();

        await host.RunAsync();
      }
      catch (Exception ex)
      {
        while (ex != null)
        {
          Console.WriteLine(ex.Message);
          Console.WriteLine(ex.StackTrace);
          ex = ex.InnerException;
        }

        Environment.Exit(1);
      }
    }
  }
}
