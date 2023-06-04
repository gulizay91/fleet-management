using System.Runtime.Serialization;

namespace Domain.Exceptions;

[Serializable]
public class DuplicateDeliveryException : ArgumentException
{
  public DuplicateDeliveryException(string message) : base(message)
  {
  }

  protected DuplicateDeliveryException(SerializationInfo info,
    StreamingContext context) : base(info, context)
  {
  }
}