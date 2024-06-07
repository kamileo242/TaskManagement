using System.Diagnostics;

namespace Models.Statueses
{
  /// <summary>
  /// Status projektu
  /// </summary>
  [DebuggerDisplay("{Value}")]
  public class ProjectStatus
  {
    /// <summary>
    /// Wartość tekstowa statusu projektu
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Stała reprezentująca nierozpoczęty projekt
    /// </summary>
    public static ProjectStatus NotStarted { get; } = new("nierozpoczęty");

    /// <summary>
    /// Stała reprezentująca rozpoczęty projekt
    /// </summary>
    public static ProjectStatus Started { get; } = new("rozpoczety");

    /// <summary>
    /// Stała reprezentująca zakończony projekt
    /// </summary>
    public static ProjectStatus Ended { get; } = new("zakonczony");

    /// <summary>
    /// Stała reprezentująca zakończony projekt
    /// </summary>
    public static ProjectStatus Canceled { get; } = new("anulowany");

    /// <summary>
    /// Sprawdzenie czy bieżący status odpowiada nierozpoczętemu projektowi
    /// </summary>
    public bool IsNotStarted => this == NotStarted;

    /// <summary>
    /// Sprawdzenie czy bieżący status odpowiada rozpoczętemu projektowi
    /// </summary>
    public bool IsStarted => this == Started;

    /// <summary>
    /// Sprawdzenie czy bieżący status odpowiada zakończonemu projektowi
    /// </summary>
    public bool IsEnded => this == Ended;

    /// <summary>
    /// Sprawdzenie czy bieżący status odpowiada anulowanemu projektowi
    /// </summary>
    public bool IsCanceled => this == Canceled;

    /// <summary>
    /// Zbiór wszytskich obsługiwanych statusów projektu
    /// </summary>
    public static ProjectStatus[] All { get; } = new[] { NotStarted, Started, Ended, Canceled };

    /// <summary>
    /// Konstruktor inicjalizujący obiekt TaskProject.
    /// </summary>
    /// <param name="value">Wartość tekstowa statusu projektu.</param>
    public ProjectStatus(string value) => Value = value;

    /// <summary>
    /// Ustalenie statusu projektu na podstawie wartości tekstowej 
    /// </summary>
    /// <param name="text">Wartość tekstowa statusu</param>
    /// <returns>Ustalony status</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ProjectStatus FromText(string text)
        => All.FirstOrDefault(s => s.Value == text) ?? throw new ArgumentOutOfRangeException(nameof(text));
  }
}
