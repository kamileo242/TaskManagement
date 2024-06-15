using Models;
using Models.Converts;
using System.Collections;

namespace Domain.Services
{
  public class HistoryUpdater : IHistoryUpdater
  {
    private readonly DataChange dataChange;

    public string ObjectType => dataChange.ObjectType;
    public string ObjectId => dataChange.ObjectId;
    public string OperationMessage { get => dataChange.OperationType; set => dataChange.OperationType = value; }
    public List<ChangeDetails> ChangeDetails => dataChange.ChangeDetails;

    public HistoryUpdater(DataChange dataChange)
    {
      this.dataChange = dataChange;
    }

    public void SetObjectId<T>(Guid? objectId)
      => SetObjectId<T>(objectId.GuidToTextOrNull());

    public void SetObjectId<T>(string objectId)
    {
      dataChange.ObjectType = typeof(T).Name;
      dataChange.ObjectId = objectId;
    }

    public void SetChangeDetails<TEntity>(TEntity originalEntity, TEntity newEntity)
    {
      dataChange.ChangeDetails = GetChanges(originalEntity, newEntity);
    }

    public void SetChangeDetailsList<TEntity>(TEntity originalEntity, TEntity newEntity)
    {
      dataChange.ChangeDetails = GetListChanges(originalEntity, newEntity);
    }

    public void SetChangeDetailsStatus(string oldStatus, string newStatus)
    {
      dataChange.ChangeDetails = GetStatusChanges(oldStatus, newStatus);
    }

    private List<ChangeDetails> GetChanges<TEntity>(TEntity originalEntity, TEntity newEntity)
    {
      var changes = new List<ChangeDetails>();

      var properties = typeof(TEntity).GetProperties();
      foreach (var property in properties)
      {
        if (originalEntity == null)
        {
          var newValue = property.GetValue(newEntity)?.ToString();

          if (!string.IsNullOrEmpty(newValue))
          {
            changes.Add(new ChangeDetails(property.Name, null, newValue));
          }
        }
        else if (newEntity == null)
        {
          var originalValue = property.GetValue(originalEntity)?.ToString();

          if (!string.IsNullOrEmpty(originalValue))
          {
            changes.Add(new ChangeDetails(property.Name, originalValue, null));
          }
        }
        else
        {
          var originalValue = property.GetValue(originalEntity)?.ToString();
          var newValue = property.GetValue(newEntity)?.ToString();

          if (!string.IsNullOrEmpty(newValue))
          {
            if (originalValue != newValue)
            {
              changes.Add(new ChangeDetails(property.Name, originalValue, newValue));
            }
          }
        }
      }
      return changes;
    }

    private List<ChangeDetails> GetListChanges<TEntity>(TEntity originalEntity, TEntity newEntity)
    {
      var changes = new List<ChangeDetails>();

      var properties = typeof(TEntity).GetProperties();
      foreach (var property in properties)
      {
        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
          var originalList = ((IEnumerable) property.GetValue(originalEntity)).Cast<object>().ToList();
          var newList = ((IEnumerable) property.GetValue(newEntity)).Cast<object>().ToList();

          if (newList.Count > originalList.Count)
          {
            var differentItems = newList.Except(originalList).ToList();
            changes.Add(new ChangeDetails(property.Name, null, string.Join(", ", differentItems)));
          }

          if (originalList.Count > newList.Count)
          {

            var differentItems = originalList.Except(newList).ToList();
            changes.Add(new ChangeDetails(property.Name, string.Join(", ", differentItems), null));
          }
        }
      }
      return changes;
    }

    private List<ChangeDetails> GetStatusChanges(string oldStatus, string newStatus)
    {
      var changes = new List<ChangeDetails>();
      if (oldStatus != newStatus)
      {
        changes.Add(new ChangeDetails("Status", oldStatus, newStatus));
      }
      return changes;
    }
  }
}
