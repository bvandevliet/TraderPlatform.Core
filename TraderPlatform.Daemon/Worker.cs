namespace TraderPlatform.Daemon;

public class Worker : BackgroundService
{
  private readonly ILogger<Worker> logger;

  public Worker(ILogger<Worker> logger)
  {
    this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
      await Task.Delay(1000, stoppingToken);
    }
  }
}