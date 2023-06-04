using System.Text.Json.Serialization;
using Domain.Enums;

namespace Api.V1.Exchanges.Requests;

public record DistributeRequest
{
  [JsonPropertyName("route")] public List<RouteModel> RouteModels { get; init; }
}

public record RouteModel
{
  [JsonPropertyName("deliveryPoint")] public DeliveryPoint DeliveryPoint { get; init; }
  [JsonPropertyName("deliveries")] public List<DeliveryModel> Deliveries { get; init; }
}

public record DeliveryModel
{
  [JsonPropertyName("barcode")] public string Barcode { get; init; }
}