using System.Text.Json.Serialization;
using Domain.Enums;

namespace Api.V1.Exchanges.Responses;

public record DistributeResponse
{
  [JsonPropertyName("vehicle")] public string Vehicle { get; set; }
  [JsonPropertyName("route")] public List<RouteModel> RouteModels { get; set; }
}

public record RouteModel
{
  [JsonPropertyName("deliveryPoint")] public DeliveryPoint DeliveryPoint { get; set; }
  [JsonPropertyName("deliveries")] public List<DeliveryModel> Deliveries { get; set; }
}

public record DeliveryModel
{
  [JsonPropertyName("barcode")] public string Barcode { get; set; }
  [JsonPropertyName("state")] public int State { get; set; }
}