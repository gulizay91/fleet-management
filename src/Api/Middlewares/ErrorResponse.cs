namespace Api.Middlewares;

public record ErrorResponse
{
  public string Message { get; init; }
}