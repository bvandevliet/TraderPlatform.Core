using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Daemon;

public class Program
{
  public static void Main(string[] args)
  {
    IHost host = Host.CreateDefaultBuilder(args)
      .ConfigureServices((builder, services) =>
      {
        services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoDatabase"));

        services.AddSingleton<IMongoClient>(x => new MongoClient(x.GetService<IOptions<MongoSettings>>()!.Value.ConnectionString));

        services.AddHostedService<Worker>();
      })
      .Build();

    host.Run();
  }
}