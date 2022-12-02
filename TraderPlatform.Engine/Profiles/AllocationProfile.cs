using AutoMapper;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Engine.Profiles;

public class AllocationProfile : Profile
{
  public AllocationProfile()
  {
    CreateMap<Allocation, AllocationDto>()
      .ForMember(
        dest => dest.QuoteSymbol, opt => opt.MapFrom(
          src => src.Market.QuoteCurrency.Symbol))
      .ForMember(
        dest => dest.BaseSymbol, opt => opt.MapFrom(
          src => src.Market.BaseCurrency.Symbol));

    CreateMap<AllocationDto, Allocation>()
      .ForCtorParam("quoteSymbol", opt => opt.MapFrom(src => src.QuoteSymbol))
      .ForCtorParam("baseSymbol", opt => opt.MapFrom(src => src.BaseSymbol))
      .ForCtorParam("price", opt => opt.MapFrom(src => src.Price))
      .ForCtorParam("amount", opt => opt.MapFrom(src => src.Amount));
  }
}