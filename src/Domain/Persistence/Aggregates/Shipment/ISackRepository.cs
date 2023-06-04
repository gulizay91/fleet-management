namespace Domain.Persistence.Aggregates.Shipment;

public interface ISackRepository
{
  public Task<Sack?> FindSackByBarcode(string barcode);
}