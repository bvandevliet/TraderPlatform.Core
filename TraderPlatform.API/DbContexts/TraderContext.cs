using Microsoft.EntityFrameworkCore;
using TraderPlatform.API.Entities;

namespace TraderPlatform.API.DbContexts;

public class TraderContext : DbContext
{
  DbSet<ConfigEntity> Configurations { get; set; } = null!;

  public TraderContext(DbContextOptions options) : base(options)
  {
  }
}