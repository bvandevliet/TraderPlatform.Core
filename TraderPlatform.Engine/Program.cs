namespace TraderPlatform.Engine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
}