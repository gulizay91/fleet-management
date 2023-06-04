namespace Domain.Persistence.Aggregates.Shipment;

public interface IPackageRepository
{
  public Task<Package?> FindPackageByBarcode(string barcode);
}