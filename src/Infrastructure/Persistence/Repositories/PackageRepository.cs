using Domain.Persistence.Aggregates.Shipment;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PackageRepository : IPackageRepository
{
  private readonly FleetDbContext _fleetDbContext;

  public PackageRepository(FleetDbContext fleetDbContext)
  {
    _fleetDbContext = fleetDbContext;
  }

  public async Task<Package?> FindPackageByBarcode(string barcode)
  {
    return await _fleetDbContext.Package.Include(r => r.Sack).FirstOrDefaultAsync(r => r.Barcode == barcode);
  }
}