using Api.Services;
using Api.Tests.UnitTests.ObjectMothers;
using Api.V1.Controllers;
using Api.V1.Exchanges.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.Tests.UnitTests.ApplicationTests.V1;

public class VehiclesControllerTest
{
  private readonly Mock<IRouteService> _mockRouteService;
  private readonly VehiclesController _sut;

  public VehiclesControllerTest()
  {
    Mock<ILogger<VehiclesController>> mockLogger = new();
    _mockRouteService = new Mock<IRouteService>();
    _sut = new VehiclesController(mockLogger.Object, _mockRouteService.Object);
  }

  [Fact]
  public async Task GetInitialData_ShouldReturn200Status()
  {
    // Act
    var result = (OkResult)await _sut.GetInitialData();

    // Assert
    result.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task Distribute_ShouldReturn200Status()
  {
    // Arrange
    const string vehiclePlate = "34TL34";
    var simpleDistributeRequest = DistributeMother.SimpleDistributeRequest();

    // Act
    var result = (OkObjectResult)await _sut.Distribute(vehiclePlate, simpleDistributeRequest);

    // Assert
    result.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task Distribute_ShouldReturn200Status_WhenInvokeRouteService()
  {
    // Arrange
    const string vehiclePlate = "34TL34";
    var simpleDistributeRequest = DistributeMother.SimpleDistributeRequest();
    var simpleDistributeResponse = DistributeMother.SimpleDistributeResponse(vehiclePlate);
    _mockRouteService.Setup(_ => _.Distribute(It.IsAny<string>(), It.IsAny<DistributeRequest>()))
      .ReturnsAsync(simpleDistributeResponse);

    // Act
    var result = (OkObjectResult)await _sut.Distribute(vehiclePlate, simpleDistributeRequest);

    // Assert
    result.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task Distribute_ShouldExcalyOnce_WhenInvokeRouteService()
  {
    // Arrange
    const string vehiclePlate = "34TL34";
    var simpleDistributeRequest = DistributeMother.SimpleDistributeRequest();
    var simpleDistributeResponse = DistributeMother.SimpleDistributeResponse(vehiclePlate);
    _mockRouteService.Setup(_ => _.Distribute(It.IsAny<string>(), It.IsAny<DistributeRequest>()))
      .ReturnsAsync(simpleDistributeResponse);

    // Act
    await _sut.Distribute(vehiclePlate, simpleDistributeRequest);

    // Assert
    _mockRouteService.Verify(service => service.Distribute(It.IsAny<string>(), It.IsAny<DistributeRequest>()),
      Times.Once());
  }
}