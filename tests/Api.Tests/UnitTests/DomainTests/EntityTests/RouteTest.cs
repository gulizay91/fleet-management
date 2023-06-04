using Domain.Constants;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Persistence.Aggregates.Route;
using Domain.Persistence.Aggregates.Shipment;
using FluentAssertions;

namespace Api.Tests.UnitTests.DomainTests.EntityTests;

public class RouteTest
{
  [Fact]
  public void RouteDelivery_ShouldLoad()
  {
    // Arrange
    var package = new Package(Barcodes.P8_988000_120, DeliveryPoint.Branch, 30);
    var route = new Route(DeliveryPoint.Branch);

    // Act
    route.LoadDelivery(package);

    // Assert
    package.State.Should().Be((int)State.Loaded);
  }
  
  [Fact]
  public void RouteDelivery_ShouldThrowShipmentNotFoundException_WhenDownloadDelivery()
  {
    // Arrange
    var package = new Package(Barcodes.P8_988000_120, DeliveryPoint.Branch, 30);
    var route = new Route(DeliveryPoint.Branch);
    
    // Act
    var throwingAction = () => { route.DownloadDelivery(package); };

    // Assert
    Assert.Throws<ShipmentNotFoundException>(throwingAction);
  }
  
  [Fact]
  public void RouteDelivery_ShouldDownload_WhenDeliveryPoint_Branch()
  {
    // Arrange
    var package = new Package(Barcodes.P8_988000_120, DeliveryPoint.Branch, 30);
    var route = new Route(DeliveryPoint.Branch);
    route.LoadDelivery(package);

    // Act
    route.DownloadDelivery(package);

    // Assert
    package.State.Should().Be((int)State.Unloaded);
  }
}