using System.Diagnostics;

namespace Models.Statueses
{
  /// <summary>
  /// Status zadania
  /// </summary>
  [DebuggerDisplay("{Value}")]
  public class TaskStatus
  {
    /// <summary>
    /// Wartość tekstowa statusu zadania
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Stała reprezentująca nierozpoczęte zadanie
    /// </summary>
    public static TaskStatus NotStarted { get; } = new("nierozpoczęte");

    /// <summary>
    /// Stała reprezentująca rozpoczęte zadanie
    /// </summary>
    public static TaskStatus Started { get; } = new("rozpoczete");

    /// <summary>
    /// Stała reprezentująca zakończone zadanie
    /// </summary>
    public static TaskStatus Ended { get; } = new("zakonczone");

    /// <summary>
    /// Sprawdzenie czy bieżący status odpowiada nierozpoczętemu zadaniu
    /// </summary>
    public bool IsNotStarted => this == NotStarted;

    /// <summary>
    /// Sprawdzenie czy bieżący status odpowiada rozpoczętemu zadaniu
    /// </summary>
    public bool IsStarted => this == Started;

    /// <summary>
    /// Sprawdzenie czy bieżący status odpowiada zakończonemu zadaniu
    /// </summary>
    public bool IsEnded => this == Ended;

    /// <summary>
    /// Zbiór wszytskich obsługiwanych statusów zadania
    /// </summary>
    public static TaskStatus[] All { get; } = new[] { NotStarted, Started, Ended };

    /// <summary>
    /// Konstruktor inicjalizujący obiekt TaskStatus.
    /// </summary>
    /// <param name="value">Wartość tekstowa statusu zadania.</param>
    public TaskStatus(string value) => Value = value;

    /// <summary>
    /// Ustalenie statusu zadania na podstawie wartości tekstowej 
    /// </summary>
    /// <param name="text">Wartość tekstowa statusu</param>
    /// <returns>Ustalony status</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static TaskStatus FromText(string text)
        => All.FirstOrDefault(s => s.Value == text) ?? throw new ArgumentOutOfRangeException(nameof(text));
  }
}
