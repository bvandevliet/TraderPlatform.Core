using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;
using System.Text.Json;
using TraderPlatform.API.Entities;

namespace TraderPlatform.API.DbContexts;

public class TraderContext : DbContext
{
  public DbSet<ConfigEntity> Configurations { get; set; } = null!;

  public TraderContext(DbContextOptions options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    foreach (PropertyInfo dbProp in typeof(TraderContext).GetProperties(BindingFlags.Instance | BindingFlags.Public))
    {
      if (!dbProp.PropertyType.Name.StartsWith("DbSet"))
      {
        continue;
      }

      foreach (Type entType in dbProp.PropertyType.GenericTypeArguments)
      {
        foreach (PropertyInfo entProp in entType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
          Type propType = entProp.PropertyType;

          void build<T>()
          {
            modelBuilder
              .Entity(entType)
              .Property(entProp.Name)
              .HasConversion(new ValueConverter<T, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<T>(v, (JsonSerializerOptions?)null)!));
          }

          if (propType == typeof(ICollection<int>))
          {
            build<ICollection<int>>();
          }
          else if (propType == typeof(ICollection<string>))
          {
            build<ICollection<string>>();
          }
          else if (propType == typeof(Dictionary<string, decimal>))
          {
            build<Dictionary<string, decimal>>();
          }
        }
      }
    }
  }
}