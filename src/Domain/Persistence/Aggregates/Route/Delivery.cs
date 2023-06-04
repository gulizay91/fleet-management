using Domain.Persistence.SeedWork;

namespace Domain.Persistence.Aggregates.Route;

public class Delivery : IEntity<Guid>
{
  public Delivery(string barcode)
  {
    Id = Guid.NewGuid();
    Barcode = barcode;
  }

  public string Barcode { get; private set; }
  public int State { get; private set; }
  public Guid Id { get; init; }

  public void SetState(int state)
  {
    State = state;
  }
}