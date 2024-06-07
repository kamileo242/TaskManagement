using System.Reflection;
using System.Text.Json.Serialization;

namespace WebApi.Dtos
{
  /// <summary>
  /// Klasa bazowa dla modeli zmian
  /// </summary>
  /// <typeparam name="T">Typ modyfikowanego obiektu</typeparam>
  public record ChangeDto<T>
  {
    /// <summary>
    /// Dane obiektu do modyfikacji
    /// </summary>
    public T Data { get; init; }

    /// <summary>
    /// Zwraca listę pół do modyfikacji
    /// </summary>
    [JsonIgnore]
    public IEnumerable<string> Updates
      => typeof(T)
      .GetProperties(BindingFlags.Public | BindingFlags.Instance)
      .Where(s => s.GetValue(Data, null) != null)
      .Select(s => s.Name);
  }
}
