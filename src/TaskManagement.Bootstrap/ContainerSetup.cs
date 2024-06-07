using Autofac;
using DataLayer;
using Domain;
using System.Reflection;

namespace Bootstrap
{
  /// <summary>
  /// Konfiguracja kontenera DI.
  /// </summary>
  internal class ContainerSetup : Autofac.Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      var domainContracts = typeof(IUserService).Assembly;
      var dataLayerContracts = typeof(IUserRepository).Assembly;

      var directory = AppContext.BaseDirectory;
      var files = Directory.GetFiles(directory, "TaskManagement*.dll");

      foreach (string file in files)
      {
        var assembly = Assembly.LoadFrom(file);

        builder
          .RegisterAssemblyTypes(assembly)
          .Where(type =>
               type.GetInterfaces().Any(
                  s => s.IsPublic
                  && (s.Assembly == assembly || s.Assembly == domainContracts || s.Assembly == dataLayerContracts)))
          .AsImplementedInterfaces()
          .SingleInstance();
      }
    }
  }
}
