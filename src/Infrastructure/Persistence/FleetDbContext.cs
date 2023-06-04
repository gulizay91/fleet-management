using Domain.Persistence.Aggregates.Route;
using Domain.Persistence.Aggregates.Shipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;

public sealed class FleetDbContext : DbContext
{
  public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options)
  {
    //Database.EnsureDeleted();
    Database.EnsureCreated();
  }

  public DbSet<Sack> Sack { get; set; } = null!;
  public DbSet<Package> Package { get; set; } = null!;
  public DbSet<Route> Route { get; set; } = null!;
  public DbSet<Delivery> Delivery { get; set; } = null!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    // ps: dataanotation or fluent api validations not working in memory db!
    // ref: https://github.com/dotnet/efcore/issues/7228
    if (!optionsBuilder.IsConfigured) optionsBuilder.UseInMemoryDatabase("fleet-management");
  }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new SackEntityTypeConfiguration());
    modelBuilder.ApplyConfiguration(new PackageEntityTypeConfiguration());

    modelBuilder.ApplyConfiguration(new RouteEntityTypeConfiguration());
    modelBuilder.ApplyConfiguration(new DeliveryEntityTypeConfiguration());
  }
}

internal class SackEntityTypeConfiguration : IEntityTypeConfiguration<Sack>
{
  public void Configure(EntityTypeBuilder<Sack> entityBuilder)
  {
    var navigation = entityBuilder.Metadata.FindNavigation(nameof(Sack.Packages));
    navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

    entityBuilder.HasKey(t => t.Id);
    entityBuilder.HasIndex(t => t.Barcode).IsUnique();
    entityBuilder.Property(t => t.State).HasConversion<int>().IsRequired().IsConcurrencyToken();
    entityBuilder.Property(t => t.DeliveryPoint).HasConversion<int>().IsRequired();
    // entityBuilder
    //   .HasMany(b => b.Packages)
    //   .WithOne()
    //   .HasForeignKey("SackId")
    //   .IsRequired(false);
    // entityBuilder
    //   .Navigation(b => b.Packages)
    //   .UsePropertyAccessMode(PropertyAccessMode.Property);
  }
}

internal class PackageEntityTypeConfiguration : IEntityTypeConfiguration<Package>
{
  public void Configure(EntityTypeBuilder<Package> entityBuilder)
  {
    entityBuilder.HasKey(t => t.Id);
    entityBuilder.Property(t => t.Id).ValueGeneratedOnAdd();
    entityBuilder.HasIndex(t => t.Barcode).IsUnique();
    //entityBuilder.Property("SackId").IsRequired(false); // shadow
    entityBuilder.Property(t => t.State).HasConversion<int>().IsRequired().IsConcurrencyToken();
    // entityBuilder.HasOne(t => t.Sack)
    //   .WithMany()
    //   .HasForeignKey("SackId").IsRequired(false);
  }
}

internal class DeliveryEntityTypeConfiguration : IEntityTypeConfiguration<Delivery>
{
  public void Configure(EntityTypeBuilder<Delivery> entityBuilder)
  {
    entityBuilder.HasKey(t => t.Id);
    entityBuilder.Property(t => t.Id).ValueGeneratedOnAdd();
    entityBuilder.Property(t => t.Barcode);
    //entityBuilder.HasIndex("RouteId", nameof(Delivery.Barcode)).IsUnique();
    //entityBuilder.Property("RouteId").IsRequired(); // shadow
    entityBuilder.Property(t => t.State).HasConversion<int>().IsRequired().IsConcurrencyToken();
  }
}

internal class RouteEntityTypeConfiguration : IEntityTypeConfiguration<Route>
{
  public void Configure(EntityTypeBuilder<Route> entityBuilder)
  {
    var navigation = entityBuilder.Metadata.FindNavigation(nameof(Route.Deliveries));
    navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

    entityBuilder.HasKey(t => t.Id);
    entityBuilder.HasIndex(t => t.DeliveryPoint).IsUnique();
    entityBuilder.Property(t => t.DeliveryPoint).HasConversion<int>();
    // entityBuilder
    //   .HasMany(b => b.Deliveries)
    //   .WithOne()
    //   .HasForeignKey("RouteId")
    //   .IsRequired(false);
    // entityBuilder
    //   .Navigation(b => b.Deliveries)
    //   .UsePropertyAccessMode(PropertyAccessMode.Property);
  }
}