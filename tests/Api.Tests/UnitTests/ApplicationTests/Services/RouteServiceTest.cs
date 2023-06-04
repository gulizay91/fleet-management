using Api.Services;
using Api.Tests.UnitTests.ObjectMothers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.Tests.UnitTests.ApplicationTests.Services;

public class RouteServiceTest
{
  private readonly Mock<ILogger<RouteService>> _mockLogger;
  private readonly RouteService _sut;

  public RouteServiceTest()
  {
    _mockLogger = new Mock<ILogger<RouteService>>();
    _sut = new RouteService(_mockLogger.Object);
  }

  [Fact]
  public async Task AddRoute_ShouldReturnDistributeResponse()
  {
    // Arrange
    const string vehiclePlate = "34TL34";
    var simpleDistribute = DistributeMother.SimpleDistributeRequest();

    // Act
    var result = await _sut.Distribute(vehiclePlate, simpleDistribute);

    // Assert
    result.Should().NotBeNull();
  }
}