using System.Runtime.Serialization;

namespace Domain.Exceptions;

[Serializable]
public class DuplicatePackageException : ArgumentException
{
  public DuplicatePackageException(string message, string paramName) : base(message, paramName)
  {
  }

  protected DuplicatePackageException(SerializationInfo info,
    StreamingContext context) : base(info, context)
  {
  }
}