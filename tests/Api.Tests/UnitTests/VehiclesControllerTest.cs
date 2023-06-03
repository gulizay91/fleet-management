using Api.V1.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.Tests.UnitTests;

public class VehiclesControllerTest
{
  private readonly VehiclesController _sut;

  public VehiclesControllerTest()
  {
    Mock<ILogger<VehiclesController>> mockLogger = new();
    _sut = new VehiclesController(mockLogger.Object);
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
    // Act
    var result = (OkResult)await _sut.Distribute("34TL34");

    // Assert
    result.StatusCode.Should().Be(200);
  }
}