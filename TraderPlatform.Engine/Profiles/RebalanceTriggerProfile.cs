using AutoMapper;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Engine.Profiles;

public class RebalanceTriggerProfile : Profile
{
  public RebalanceTriggerProfile()
  {
    CreateMap<AbsAssetAllocDto, AbsAssetAlloc>()
      .ForCtorParam("baseSymbol", opt => opt.MapFrom(src => src.BaseSymbol))
      .ForCtorParam("absAlloc", opt => opt.MapFrom(src => src.AbsAlloc))
      .ReverseMap();
  }
}