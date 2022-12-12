using Microsoft.Net.Http.Headers;
using System.Net.Http.Json;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Common.HttpClients;

public class EngineClient
{
  private readonly HttpClient httpClient;

  public EngineClient(HttpClient httpClient)
  {
    this.httpClient = httpClient;

    string address = Environment.GetEnvironmentVariable("ADDRESS_ENGINE")!;

    this.httpClient.BaseAddress = new($"{address}/api/");

    this.httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
  }

  public async Task<IEnumerable<OrderDto>?> Rebalance(RebalanceTriggerDto rebalanceTrigger)
  {
    HttpResponseMessage response =
      await httpClient.PostAsJsonAsync("rebalance", rebalanceTrigger);

    return await response.Content.ReadFromJsonAsync<IEnumerable<OrderDto>>();
  }
}