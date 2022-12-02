using AutoMapper;
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
  private readonly IMapper mapper;
  private readonly IExchangeService exchangeService;

  public RebalanceController(
    IMapper mapper,
    IExchangeService exchangeService)
  {
    this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    this.exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
  }

  [HttpPost]
  public async Task<ActionResult<IEnumerable<OrderDto>>> Rebalance(
    IEnumerable<AbsAssetAllocDto> newAssetAllocs)
  {
    IEnumerable<Abstracts.Interfaces.IOrder> results =
      await exchangeService.Rebalance(mapper.Map<List<AbsAssetAlloc>>(newAssetAllocs));

    return Ok(mapper.Map<IEnumerable<OrderDto>>(results));
  }
}