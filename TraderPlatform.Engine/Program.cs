using TraderPlatform.Abstracts.Services;
using TraderPlatform.Common.Exchanges;

namespace TraderPlatform.Engine;

public class Program
{
  public static void Main(string[] args)
  {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddRouting(options =>
    {
      options.LowercaseUrls = true;
    });

    builder.Services.AddControllers();

    // https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    if (builder.Environment.IsDevelopment())
    {
      builder.Services.AddHttpClient<BitvavoExchange>();
      builder.Services.AddSingleton<IExchangeService, MockExchange<BitvavoExchange>>();
    }
    else
    {
      //builder.Services.AddHttpClient<IExchangeService, ExchangeA>();
      //builder.Services.AddHttpClient<IExchangeService, ExchangeB>();
      //builder.Services.AddHttpClient<IExchangeService, ExchangeC>();
    }

    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();
  }
}