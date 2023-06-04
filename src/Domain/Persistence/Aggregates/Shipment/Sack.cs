using Domain.Enums;
using Domain.Persistence.SeedWork;

namespace Domain.Persistence.Aggregates.Shipment;

public class Sack : IShipment, IEntity<Guid>, IAggregateRoot
{
  private readonly List<Package> _packages = new();

  public Sack(string barcode, DeliveryPoint deliveryPoint)
  {
    Id = Guid.NewGuid();
    Barcode = barcode;
    DeliveryPoint = deliveryPoint;
    State = (int)SackState.Created;
  }

  public Sack(string barcode, DeliveryPoint deliveryPoint, List<Package> packages)
  {
    Id = Guid.NewGuid();
    Barcode = barcode;
    DeliveryPoint = deliveryPoint;
    State = (int)SackState.Created;

    AddPackages(packages);
  }

  public int State { get; set; }
  public IReadOnlyCollection<Package> Packages => _packages.AsReadOnly();
  public Guid Id { get; init; }
  public string Barcode { get; init; }
  public DeliveryPoint DeliveryPoint { get; protected set; }
  public ShipmentType ShipmentType => ShipmentType.Sack;

  public int GetState()
  {
    return State;
  }

  public void SetState(int state)
  {
    State = state;
    UpdateStateAllPackages();
  }

  public bool AllowDownload(DeliveryPoint deliveryPoint)
  {
    if (deliveryPoint == DeliveryPoint.Branch) return false;
    if (deliveryPoint != DeliveryPoint) return false;

    SetState((int)Enums.State.Unloaded);
    return true;
  }

  public void SyncStateByPackages(int state)
  {
    if (State == state) return;
    if (_packages.All(r => r.State == state))
      SetState(state);
  }

  public void UpdateStateAllPackages()
  {
    if (State is (int)SackState.Unloaded or (int)SackState.Loaded && _packages.Any(r => r.State != State))
      _packages.ForEach(r => r.SetState(State));
  }

  public void AddPackage(Package package)
  {
    Guard.Against.DuplicatePackage(_packages, package, nameof(Package));
    package.SetState((int)PackageState.LoadedIntoSack);
    _packages.Add(package);
  }

  public void AddPackages(List<Package> packages)
  {
    foreach (var package in packages) AddPackage(package);
  }
}