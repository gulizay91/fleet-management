namespace Domain.Persistence.SeedWork;

public interface IGuard
{
}

public class Guard : IGuard
{
  private Guard()
  {
  }

  public static IGuard Against { get; } = new Guard();
}