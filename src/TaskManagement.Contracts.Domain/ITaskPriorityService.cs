using TaskManagement.Models;

namespace Domain
{
  public interface ITaskPriorityService
  {
    /// <summary>
    /// Identyfikator priorytetu domyślnego.
    /// </summary>
    string DefaultPriority { get; }

    /// <summary>
    /// Lista wszystkich identyfikatorów priorytetów
    /// </summary>
    IEnumerable<string> AllPriorityIds { get; }

    /// <summary>
    /// Lista wszystkich identyfikatorów priorytetów
    /// </summary>
    IEnumerable<TaskPriority> AllPriority { get; }

    /// <summary>
    /// Pobierz nazwę priorytetu na podstawie jego identyfikatora.
    /// </summary>
    /// <param name="priorityId">Identyfikator priorytetu</param>
    /// <returns>Nazwa priorytetu</returns>
    string GetPriorityName(string priorityId);

    /// <summary>
    /// Odczyt informacji o priorytecie na podstawie jego identyfikatora
    /// </summary>
    /// <param name="priorityId">Identyfikator priorytetu</param>
    /// <returns>Priorytet zadania</returns>
    TaskPriority GetPriority(string priorityId);

    /// <summary>
    /// Walidacja czy identyfikator priorytetu znajduję się w słowniku priorytetu.
    /// </summary>
    /// <param name="priorityId">Identyfikator priorytetu</param>
    void ValidatePriorityId(string priorityId);
  }
}
