using System.Runtime.Serialization;

namespace TaskManagement.Models.Exceptions
{
  /// <summary>
  /// Klasa wyjątku odpowiedzialnego za nieprawidłowe dane.
  /// </summary>
  public class IncorrectDataException : BaseException
  {
    public IncorrectDataException(string message = null) : base(message) { }

    public IncorrectDataException(Exception innerException, string message = null) : base(innerException, message) { }

    public IncorrectDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
