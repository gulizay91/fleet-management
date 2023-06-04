using Domain.Constants;
using Domain.Enums;
using Domain.Persistence.Aggregates.Shipment;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class FleetDbContextSeed
{
  private readonly IEnumerable<Package> _defaultPackages;
  private readonly IEnumerable<Sack> _defaultSacks;
  private readonly FleetDbContext _fleetDbContext;

  public FleetDbContextSeed(FleetDbContext fleetDbContext)
  {
    _fleetDbContext = fleetDbContext;
    _defaultPackages = GetDefaultPackages();
    _defaultSacks = GetDefaultSacks();
  }

  public async Task Seed()
  {
    if (!_fleetDbContext.Package.Any())
    {
      _fleetDbContext.Package.AddRange(_defaultPackages);
      _fleetDbContext.Sack.AddRange(_defaultSacks);
      await _fleetDbContext.SaveChangesAsync();
    }

    InitLoadPackageToSacks();
    await InitialDataLog();
  }

  private static IEnumerable<Package> GetDefaultPackages()
  {
    var packageData = new List<Package>();
    packageData.AddRange(GetBranchPackages());
    packageData.AddRange(GetDistributionCentrePackages());
    packageData.AddRange(GetTransferCentrePackages());
    return packageData;
  }

  private static IEnumerable<Package> GetBranchPackages()
  {
    return new List<Package>
    {
      new(Barcodes.P7_988000_121, DeliveryPoint.Branch, 5),
      new(Barcodes.P7_988000_122, DeliveryPoint.Branch, 5),
      new(Barcodes.P7_988000_123, DeliveryPoint.Branch, 9)
    };
  }

  private static IEnumerable<Package> GetDistributionCentrePackages()
  {
    return new List<Package>
    {
      new(Barcodes.P8_988000_120, DeliveryPoint.DistributionCentre, 33),
      new(Barcodes.P8_988000_121, DeliveryPoint.DistributionCentre, 17),
      new(Barcodes.P8_988000_122, DeliveryPoint.DistributionCentre, 26),
      new(Barcodes.P8_988000_123, DeliveryPoint.DistributionCentre, 35),
      new(Barcodes.P8_988000_124, DeliveryPoint.DistributionCentre, 1),
      new(Barcodes.P8_988000_125, DeliveryPoint.DistributionCentre, 200),
      new(Barcodes.P8_988000_126, DeliveryPoint.DistributionCentre, 50)
    };
  }

  private static IEnumerable<Package> GetTransferCentrePackages()
  {
    return new List<Package>
    {
      new(Barcodes.P9_988000_126, DeliveryPoint.TransferCentre, 15),
      new(Barcodes.P9_988000_127, DeliveryPoint.TransferCentre, 16),
      new(Barcodes.P9_988000_128, DeliveryPoint.TransferCentre, 55),
      new(Barcodes.P9_988000_129, DeliveryPoint.TransferCentre, 28),
      new(Barcodes.P9_988000_130, DeliveryPoint.TransferCentre, 17)
    };
  }

  private static IEnumerable<Sack> GetDefaultSacks()
  {
    var sackData = new List<Sack>();
    sackData.AddRange(GetSacks());
    return sackData;
  }

  private static IEnumerable<Sack> GetSacks()
  {
    return new List<Sack>
    {
      new(Barcodes.C725799, DeliveryPoint.DistributionCentre, new List<Package>()),
      new(Barcodes.C725800, DeliveryPoint.TransferCentre, new List<Package>())
    };
  }

  private void InitLoadPackageToSacks()
  {
    if (_fleetDbContext.Sack.Any())
    {
      var sackInitFirst = _fleetDbContext.Sack.FirstOrDefault(r => r.Barcode == Barcodes.C725799);
      if (sackInitFirst is not null)
      {
        var packageOne = _defaultPackages.FirstOrDefault(r => r.Barcode == Barcodes.P8_988000_122);
        packageOne?.SetState((int)PackageState.LoadedIntoSack);
        var packageTwo = _defaultPackages.FirstOrDefault(r => r.Barcode == Barcodes.P8_988000_126);
        packageTwo?.SetState((int)PackageState.LoadedIntoSack);
        sackInitFirst.AddPackages(new List<Package> { packageOne, packageTwo });
        //sackInitFirst.AddPackage(packageOne);
        //sackInitFirst.AddPackage(packageTwo);
        //_fleetDbContext.Sack.Update(sackInitFirst);
      }

      var sackInitSecond = _fleetDbContext.Sack.FirstOrDefault(r => r.Barcode == Barcodes.C725800);
      if (sackInitSecond is not null)
      {
        var packageOne = _defaultPackages.FirstOrDefault(r => r.Barcode == Barcodes.P9_988000_128);
        packageOne?.SetState((int)PackageState.LoadedIntoSack);
        var packageTwo = _defaultPackages.FirstOrDefault(r => r.Barcode == Barcodes.P9_988000_129);
        packageTwo?.SetState((int)PackageState.LoadedIntoSack);
        sackInitSecond.AddPackages(new List<Package> { packageOne, packageTwo });
        // sackInitSecond.AddPackage(packageOne);
        // sackInitSecond.AddPackage(packageTwo);
      }

      _fleetDbContext.Sack.UpdateRange();
      _fleetDbContext.SaveChanges();
    }
  }

  private async Task InitialDataLog()
  {
    Console.WriteLine("************AllPackages**********************");
    Console.WriteLine("Barcode;DeliveryPoint;Desi;State");
    foreach (var package in await _fleetDbContext.Package.ToListAsync())
      Console.WriteLine($"{package.Barcode};{package.DeliveryPoint};{package.Desi};{(PackageState)package.State}");
    Console.WriteLine("************AllSacks**********************");
    Console.WriteLine("Barcode;DeliveryPoint;State");
    foreach (var sack in await _fleetDbContext.Sack.ToListAsync())
      Console.WriteLine($"{sack.Barcode};{sack.DeliveryPoint};{(SackState)sack.State}");
    Console.WriteLine("************AllSackPackages**********************");
    Console.WriteLine("SackBarcode;SackState;PackageBarcode;PackageState");
    foreach (var sack in await _fleetDbContext.Sack.Include(r => r.Packages).ToListAsync())
    foreach (var package in sack.Packages)
      Console.WriteLine($"{sack.Barcode};{(SackState)sack.State};{package.Barcode};{(PackageState)package.State}");
  }
}