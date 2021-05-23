using System.Text.Json.Serialization;
using Bank.App.Accounts.Queries;
using Bank.Data.Infrastructure;
using Bank.Data.Repositories;
using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Bank.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers()
        .AddJsonOptions(opt => opt
          .JsonSerializerOptions
          .Converters.Add(new JsonStringEnumConverter())
        );
      CustomServices(services);
    }

    public void Configure(
      IApplicationBuilder app,
      IWebHostEnvironment env,
      ILogger<Startup> logger)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      TryConfig.ErrorLogger = e => logger.LogError(e.Message);

      app.UseRouting()
        .UseEndpoints(endpoints =>
          endpoints.MapControllers());
    }

    private static void CustomServices(IServiceCollection services) =>
      services
        .AddMediatR(typeof(GetAccountById))
        .AddSingleton<IMongoClient, MongoClient>()
        .AddSingleton<IMongoClientProvider, MongoClientProvider>()
        .AddScoped<IAccountRepository, AccountRepository>();
  }
}
