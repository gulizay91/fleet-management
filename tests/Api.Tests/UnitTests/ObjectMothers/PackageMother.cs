using Domain.Constants;
using Domain.Enums;
using Domain.Persistence.Aggregates.Shipment;

namespace Api.Tests.UnitTests.ObjectMothers;

public class PackageMother
{
  public static Package SimplePackage()
  {
    return new Package(Barcodes.P7_988000_121, DeliveryPoint.Branch, 5);
  }
}