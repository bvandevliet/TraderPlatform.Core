using AutoMapper;
using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Engine.Profiles;

public class OrderProfile : Profile
{
  public OrderProfile()
  {
    CreateMap<IOrder, OrderDto>().IncludeAllDerived()
      .ForMember(
        dest => dest.QuoteSymbol, opt => opt.MapFrom(
          src => src.Market.QuoteCurrency.Symbol))
      .ForMember(
        dest => dest.BaseSymbol, opt => opt.MapFrom(
          src => src.Market.BaseCurrency.Symbol));

    CreateMap<OrderDto, Order>()
      .ForCtorParam("quoteSymbol", opt => opt.MapFrom(src => src.QuoteSymbol))
      .ForCtorParam("baseSymbol", opt => opt.MapFrom(src => src.BaseSymbol))
      .ForCtorParam("side", opt => opt.MapFrom(src => src.Side))
      .ForCtorParam("type", opt => opt.MapFrom(src => src.Type))
      .ForCtorParam("amountQuote", opt => opt.MapFrom(src => src.AmountQuote))
      .ForCtorParam("amount", opt => opt.MapFrom(src => src.Amount))
      .ForCtorParam("price", opt => opt.MapFrom(src => src.Price));
  }
}