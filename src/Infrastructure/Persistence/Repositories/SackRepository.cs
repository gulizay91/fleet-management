using Domain.Persistence.Aggregates.Shipment;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SackRepository : ISackRepository
{
  private readonly FleetDbContext _fleetDbContext;

  public SackRepository(FleetDbContext fleetDbContext)
  {
    _fleetDbContext = fleetDbContext;
  }

  public async Task<Sack?> FindSackByBarcode(string barcode)
  {
    return await _fleetDbContext.Sack.Include(r => r.Packages).FirstOrDefaultAsync(r => r.Barcode == barcode);
  }
}