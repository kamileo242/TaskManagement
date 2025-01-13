using System.Runtime.Serialization;

namespace TaskManagement.Models.Exceptions
{
  /// <summary>
  /// Klasa bazowa dla wyjątków
  /// </summary>
  public abstract class BaseException : Exception
  {
    /// <summary>
    /// Utworzenie wyjątku z komunikatem.
    /// </summary>
    /// <param name="message">Komunikat wyjątku</param>
    protected BaseException(string message = null) : base(message) { }

    /// <summary>
    /// Utworzenie wyjątku z komunikatem z dorzuceniem innego wyjątku.
    /// </summary>
    /// <param name="message">Komunikat błędu</param>
    /// <param name="innerException">Wewnętrzny obiekt wyjątku</param>
    protected BaseException(Exception innerException, string message = null) : base(message, innerException) { }

    protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
