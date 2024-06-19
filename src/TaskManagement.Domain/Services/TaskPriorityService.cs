using TaskManagement.Models;

namespace Domain.Services
{
  public class TaskPriorityService : ITaskPriorityService
  {
    private const string Default = "normalny";
    private const string Low = "niski";
    private const string High = "wysoki";
    private const string Critical = "krytyczny";

    private static readonly TaskPriority[] priorities = new TaskPriority[]
    {
      new () { Id = Low, Name = "Niski" },
      new () { Id = Default, Name = "Normalny" },
      new () { Id = High, Name = "Wysoki" },
      new () { Id = Critical, Name = "Krytyczny" },
    };

    private static readonly string[] priorityIds = priorities.Select(p => p.Id).ToArray();

    private static readonly Dictionary<string, TaskPriority> priorityById = priorities.ToDictionary(p => p.Id, p => p);


    public string DefaultPriority { get; } = Default;
    public IEnumerable<string> AllPriorityIds => priorityIds;
    public IEnumerable<TaskPriority> AllPriority => priorities;

    public TaskPriority GetPriority(string priorityId)
      => priorityById.TryGetValue(priorityId, out var priority)
      ? priority
      : null;

    public string GetPriorityName(string priorityId)
      => priorityById.TryGetValue(priorityId, out var priority)
      ? priority.Name
      : string.Empty;

    public void ValidatePriorityId(string priorityId)
    {
      if (!string.IsNullOrEmpty(priorityId) && !priorityById.ContainsKey(priorityId))
      {
        throw new InvalidDataException($"Nieprawidłowy identyfikator priorytetu: {priorityId}");
      }
    }
  }
}
