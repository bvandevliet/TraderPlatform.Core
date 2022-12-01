using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Services;
using TraderPlatform.Engine.Core;

namespace TraderPlatform.Engine.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RebalanceController : ControllerBase
{
  private readonly IExchangeService exchangeService;

  public RebalanceController(
    IExchangeService exchangeService)
  {
    this.exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
  }

  [HttpPost]
  public async Task<ActionResult<IEnumerable<OrderDto>>> Rebalance(
    RebalanceTriggerDto rebalanceTrigger)
  {
    // THESE CASTS DON'T WORK BUT IMPLEMENTING INTERFACES INTO DTO'S DOESN'T WORK EITHER !!
    IEnumerable<Abstracts.Interfaces.IOrder> results = await exchangeService.Rebalance(
      (IEnumerable<AbsAssetAlloc>)rebalanceTrigger.NewAssetAllocs,
      (IEnumerable<KeyValuePair<Allocation, decimal>>?)rebalanceTrigger.AllocQuoteDiffs);

    return Ok(results);
  }
}