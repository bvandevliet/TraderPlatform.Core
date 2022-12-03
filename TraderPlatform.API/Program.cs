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

    builder.Services.AddControllers(options =>
    {
      options.ReturnHttpNotAcceptable = true;
    })
      .AddXmlDataContractSerializerFormatters();

    // https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddSingleton<HttpClient>();

    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseRouting(); // app.MapControllers(); manual step 1/2

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseEndpoints(endpoints => // app.MapControllers(); manual step 2/2
    {
      endpoints.MapControllers();
    });

    app.Run();
  }
}