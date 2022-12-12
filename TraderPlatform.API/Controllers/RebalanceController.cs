using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraderPlatform.Abstracts.Models;
using TraderPlatform.Common.HttpClients;

namespace TraderPlatform.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RebalanceController : ControllerBase
{
  private readonly EngineClient engineClient;

  public RebalanceController(
    EngineClient engineClient)
  {
    this.engineClient = engineClient ?? throw new ArgumentNullException(nameof(engineClient));
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
    return Ok(await engineClient.Rebalance(rebalanceTrigger));
  }
}