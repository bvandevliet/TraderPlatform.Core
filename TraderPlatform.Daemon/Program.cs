using MongoDB.Driver;

namespace TraderPlatform.Daemon;

public class Program
{
  public static void Main(string[] args)
  {
    IHost host = Host.CreateDefaultBuilder(args)
      .ConfigureServices((builder, services) =>
      {
        services.AddSingleton<IMongoClient>(new MongoClient(builder.Configuration.GetConnectionString("mongodb")));

        services.AddHostedService<Worker>();
      })
      .Build();

    host.Run();
  }
}