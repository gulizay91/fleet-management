using Api.Tests.UnitTests.ObjectMothers;
using Api.V1.Exchanges.Validations;
using FluentValidation.TestHelper;

namespace Api.Tests.UnitTests.ApplicationTests.V1;

public class DistributeRequestValidatorTest
{
  private readonly DistributeRequestValidator _sut;
  private readonly RouteModelValidator _sutRouteModel;

  public DistributeRequestValidatorTest()
  {
    _sut = new DistributeRequestValidator();
    _sutRouteModel = new RouteModelValidator();
  }

  [Fact]
  public async Task DistributeRequest_ShouldHaveError_WhenEmptyRoute()
  {
    // Arrange
    var simpleBadRequest = DistributeMother.SimpleBadDistributeRequest_EmptyRoute();

    // Act
    var result = await _sut.TestValidateAsync(simpleBadRequest);

    // Assert
    result.ShouldHaveValidationErrorFor(r => r.RouteModels);
  }

  [Fact]
  public async Task DistributeRequest_ShouldHaveError_WhenEmptyDelivery()
  {
    // Arrange
    var simpleBadRequest = DistributeMother.SimpleBadDistributeRequest_EmptyDelivery();

    // Act
    var result = await _sutRouteModel.TestValidateAsync(simpleBadRequest);

    // Assert
    result.ShouldHaveValidationErrorFor(r => r.Deliveries);
  }
}