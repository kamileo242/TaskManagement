using Microsoft.AspNetCore.Localization;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace TaskManagement.WebApiHost
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(o =>
      {
        o.AddPolicy("CustomPolicy", builder =>
        {
          builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
        });
      });

      services.Configure<RequestLocalizationOptions>(o =>
      {
        o.DefaultRequestCulture = new RequestCulture("pl-PL");
      });

      services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseCors("MyPolicy");
      app.UseRouting();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });


      app.UseSwagger();
      app.UseSwaggerUI(options =>
      {
        options.SwaggerEndpoint("/swagger/1.0/swagger.json", "Test");
        options.EnableFilter();
        options.DocExpansion(DocExpansion.None);
        options.DisplayOperationId();
      });
    }
  }
}
