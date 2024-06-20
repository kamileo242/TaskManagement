using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using WebApi.Converts;

namespace WebApi
{
  public class Startup
  {
    private readonly IWebHostEnvironment env;
    public IConfiguration Configuration { get; }

    public Startup(IWebHostEnvironment env, IConfiguration configuration)
    {
      this.env = env;
      Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      Console.WriteLine($"Environment: {env.EnvironmentName}");
      Console.WriteLine($"Database Name: {Configuration.GetSection("Database:DatabaseName").Value}");

      services.Configure<RequestLocalizationOptions>(o =>
      {
        o.DefaultRequestCulture = new RequestCulture("pl-PL");
      });

      services.AddControllers().AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
      });

      services.AddEndpointsApiExplorer();

      services.AddCors(o =>
      {
        o.AddPolicy("CustomPolicy", builder =>
        {
          builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
        });
      });

      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("1.0", new OpenApiInfo
        {
          Title = "TaskManagement",
          Description = "Aplikacja do zarządzania projektami.",
          Version = "1.0"
        });

        if (env.IsDevelopment() || Debugger.IsAttached)
        {
          options.ExampleFilters();
        }

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        options.EnableAnnotations();
        options.DescribeAllParametersInCamelCase();
      });

      services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
      services.AddSingleton<IDtoBuilder, DtoBuilder>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseCors("CustomPolicy");
      app.UseRouting();

      if (env.IsDevelopment() || Debugger.IsAttached)
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
          options.SwaggerEndpoint("/swagger/1.0/swagger.json", "taskmanagement 1.0");
          options.EnableFilter();
          options.DocExpansion(DocExpansion.None);
          options.DisplayOperationId();
        });
      }
      else
      {
        app.UseExceptionHandler("/error");
      }

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
