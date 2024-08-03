using System.Runtime.Serialization;

namespace TaskManagement.Models.Exceptions
{
  /// <summary>
  /// Klasa wyjątku odpowiedzialnego za brak danych.
  /// </summary>
  public class MissingDataException : BaseException
  {
    public MissingDataException(string message = null) : base(message) { }

    public MissingDataException(Exception innerException, string message = null) : base(innerException, message) { }

    public MissingDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
