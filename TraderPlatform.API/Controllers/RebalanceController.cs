using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RebalanceController : ControllerBase
{
  private readonly HttpClient httpClient;

  public RebalanceController(
    HttpClient httpClient)
  {
    this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
  }

  /// <summary>
  /// Performs a portfolio rebalance.
  /// </summary>
  /// <remarks>
  /// Directly forwards the request to the Engine/api/rebalance endpoint.
  /// </remarks>
  /// <param name="rebalanceTrigger"></param>
  /// <returns></returns>
  [HttpPost]
  public async Task<ActionResult<IEnumerable<OrderDto>>> Rebalance(
    RebalanceTriggerDto rebalanceTrigger)
  {
    var port = Environment.GetEnvironmentVariable("PORT_ENGINE");

    HttpResponseMessage response =
      await httpClient.PostAsJsonAsync($"http://traderplatform.engine:{port}/api/rebalance", rebalanceTrigger);

    return Ok(await response.Content.ReadFromJsonAsync<IEnumerable<OrderDto>>());
  }
}