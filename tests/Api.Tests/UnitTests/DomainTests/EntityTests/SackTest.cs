using Domain.Constants;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Persistence.Aggregates.Shipment;
using FluentAssertions;

namespace Api.Tests.UnitTests.DomainTests.EntityTests;

public class SackTest
{
  [Fact]
  public void Sack_ShouldAllowDownload_WhenDeliveryPoint_TransferCentre()
  {
    // Arrange
    var sack = new Sack(Barcodes.C725800, DeliveryPoint.TransferCentre);

    // Act
    var result = sack.AllowDownload(DeliveryPoint.TransferCentre);

    // Assert
    result.Should().BeTrue();
  }

  [Fact]
  public void Sack_ShouldAllowDownload_WhenDeliveryPoint_DistributionCentre()
  {
    // Arrange
    var sack = new Sack(Barcodes.C725800, DeliveryPoint.DistributionCentre);

    // Act
    var result = sack.AllowDownload(DeliveryPoint.DistributionCentre);

    // Assert
    result.Should().BeTrue();
  }

  [Fact]
  public void Sack_ShouldNotAllowDownload_WhenDeliveryPoint_Branch()
  {
    // Arrange
    var sack = new Sack(Barcodes.C725800, DeliveryPoint.TransferCentre);

    // Act
    var result = sack.AllowDownload(DeliveryPoint.Branch);

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void Sack_ShouldNotAllowDownload_WhenDeliveryPoint_InCorrectDestination()
  {
    // Arrange
    var sack = new Sack(Barcodes.C725800, DeliveryPoint.DistributionCentre);

    // Act
    var result = sack.AllowDownload(DeliveryPoint.TransferCentre);

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void AddPackage_ShouldLoadToSack()
  {
    // Arrange
    var sack = new Sack(Barcodes.C725800, DeliveryPoint.DistributionCentre);
    var package = new Package(Barcodes.P8_988000_120, DeliveryPoint.Branch, 30);

    // Act
    sack.AddPackage(package);

    // Assert
    sack.Packages.Count.Should().Be(1);
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

  [Fact]
  public void AddPackage_ShouldThrowDuplicatePackage()
  {
    // Arrange
    var sack = new Sack(Barcodes.C725800, DeliveryPoint.DistributionCentre);
    var packageOne = new Package(Barcodes.P8_988000_120, DeliveryPoint.Branch, 30);
    var packageSecond = new Package(Barcodes.P8_988000_120, DeliveryPoint.DistributionCentre, 30);
    sack.AddPackage(packageOne);

    // Act
    var throwingAction = () => { sack.AddPackage(packageSecond); };

    // Assert
    Assert.Throws<DuplicatePackageException>(throwingAction);
  }
}