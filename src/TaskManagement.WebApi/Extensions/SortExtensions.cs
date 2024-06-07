using Models;

namespace WebApi.Extensions
{
  public static class SortExtension
  {
    public static SortPart[] GetSortParts(string sort)
    {
      var sortCriteria = sort?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

      var sorting = new List<SortPart>();
      var sortedProperties = new Dictionary<string, string>();

      foreach (var criterion in sortCriteria)
      {
        var parts = criterion.Split(',');

        if (parts.Length == 2)
        {
          var name = parts[0];
          var direction = parts[1].ToLower();

          if (sortedProperties.ContainsKey(name))
          {
            throw new InvalidDataException($"Nie można sortować właściwości '{name}' w wielu kierunkach jednocześnie.");
          }

          sortedProperties[name] = direction;

          if (direction == "asc")
            sorting.Add(SortPart.Asc(name));
          else if (direction == "desc")
            sorting.Add(SortPart.Desc(name));
          else
            throw new InvalidDataException("Nieprawidłowy kierunek sortowania.");
        }
        else
        {
          throw new InvalidDataException("Nieprawidłowy format kryteriów sortowania.");
        }
      }

      return sorting.ToArray();
    }
  }
}
