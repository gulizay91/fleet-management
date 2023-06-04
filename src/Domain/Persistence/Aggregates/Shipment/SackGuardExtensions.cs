using Domain.Exceptions;
using Domain.Persistence.SeedWork;

namespace Domain.Persistence.Aggregates.Shipment;

public static class SackGuardExtensions
{
  public static void DuplicatePackage(this IGuard guard, IEnumerable<Package> existingPackages, Package newPackage,
    string parameterName)
  {
    if (existingPackages.Any(a => a.Barcode == newPackage.Barcode))
      throw new DuplicatePackageException("Cannot add duplicate package to sack.", parameterName);
  }
}