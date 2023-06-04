using Api.Services;
using Api.Tests.UnitTests.ObjectMothers;
using Domain.Persistence.Aggregates.Route;
using Domain.Persistence.Aggregates.Shipment;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.Tests.UnitTests.ApplicationTests.Services;

public class RouteServiceTest
{
  private readonly Mock<ILogger<RouteService>> _mockLogger;
  private readonly Mock<IPackageRepository> _mockPackageRepository;
  private readonly Mock<IRouteRepository> _mockRouteRepository;
  private readonly Mock<ISackRepository> _mockSackRepository;
  private readonly RouteService _sut;

  public RouteServiceTest()
  {
    _mockLogger = new Mock<ILogger<RouteService>>();
    _mockPackageRepository = new Mock<IPackageRepository>();
    _mockRouteRepository = new Mock<IRouteRepository>();
    _mockSackRepository = new Mock<ISackRepository>();
    _sut = new RouteService(_mockLogger.Object, _mockRouteRepository.Object, _mockSackRepository.Object, _mockPackageRepository.Object);
  }

  [Fact]
  public async Task AddRoute_ShouldReturnDistributeResponse()
  {
    // Arrange
    const string vehiclePlate = "34TL34";
    var simplePackage = PackageMother.SimplePackage();
    var simpleDistribute = DistributeMother.SimpleDistributeRequest();

    _mockPackageRepository
      .Setup(_ => _.FindPackageByBarcode(It.IsAny<string>()))
      .ReturnsAsync(simplePackage);

    // Act
    var result = await _sut.Distribute(vehiclePlate, simpleDistribute);

    // Assert
    result.Should().NotBeNull();
  }
}