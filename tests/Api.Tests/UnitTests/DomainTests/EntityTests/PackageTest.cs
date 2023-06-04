using Domain.Constants;
using Domain.Enums;
using Domain.Persistence.Aggregates.Shipment;
using FluentAssertions;

namespace Api.Tests.UnitTests.DomainTests.EntityTests;

public class PackageTest
{
  [Fact]
  public void PackageNotInSack_ShouldAllowDownload_WhenDeliveryPoint_Branch()
  {
    // Arrange
    var package = new Package(Barcodes.P7_988000_121, DeliveryPoint.Branch, 5);

    // Act
    var result = package.AllowDownload(DeliveryPoint.Branch);

    // Assert
    result.Should().BeTrue();
  }

  [Fact]
  public void PackageNotInSack_ShouldAllowDownload_WhenDeliveryPoint_DistributionCentre()
  {
    // Arrange
    var package = new Package(Barcodes.P7_988000_121, DeliveryPoint.DistributionCentre, 5);

    // Act
    var result = package.AllowDownload(DeliveryPoint.DistributionCentre);

    // Assert
    result.Should().BeTrue();
  }

  [Fact]
  public void PackageNotInSack_ShouldNotAllowDownload_WhenDeliveryPoint_InCorrectDestination()
  {
    // Arrange
    var package = new Package(Barcodes.P7_988000_121, DeliveryPoint.DistributionCentre, 5);

    // Act
    var result = package.AllowDownload(DeliveryPoint.Branch);

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void PackageNotInSack_ShouldNotAllowDownload_WhenDeliveryPoint_TransferCenter()
  {
    // Arrange
    var package = new Package(Barcodes.P7_988000_121, DeliveryPoint.Branch, 5);

    // Act
    var result = package.AllowDownload(DeliveryPoint.TransferCentre);

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void AddPackage_ShouldPackageState_LoadInToSack()
  {
    // Arrange
    var sack = new Sack(Barcodes.C725800, DeliveryPoint.DistributionCentre);
    var package = new Package(Barcodes.P8_988000_120, DeliveryPoint.Branch, 30);

    // Act
    sack.AddPackage(package);

    // Assert
    package.State.Should().Be((int)PackageState.LoadedIntoSack);
  }
}