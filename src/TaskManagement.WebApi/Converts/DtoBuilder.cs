using Domain;
using Models;
using Models.Converts;
using TaskManagement.WebApi.Dtos;
using WebApi.Dtos;

namespace WebApi.Converts
{
  public class DtoBuilder : IDtoBuilder
  {
    private readonly ITaskService taskService;
    private readonly IUserService userService;
    private readonly ITaskPriorityService taskPriorityService;

    public DtoBuilder(ITaskService taskService, IUserService userService, ITaskPriorityService taskPriorityService)
    {
      this.taskService = taskService;
      this.userService = userService;
      this.taskPriorityService = taskPriorityService;
    }
    public CommentDto ConvertToCommentDto(Comment model)
    {
      if (model != null)
      {
        var comment = new CommentDto
        {
          Id = model.Id.GuidToText(),
          Content = model.Content,
          CreatedAt = model.CreatedAt,
          Author = model.Author,
        };
        return comment;
      }
      return null;
    }

    public ProjectDto ConvertToProjectDto(Project model)
      => new()
      {
        Id = model.Id.GuidToText(),
        Description = model.Description,
        Deadline = model.Deadline,
        Comments = model.Comments.Select(s => ConvertToCommentDto(s)),
        CreatedAt = model.CreatedAt,
        Status = model.Status.Value.ToString(),
        Tasks = ConvertTaskIdsToTaskDtos(model.TaskIds),
        Title = model.Title,
      };

    public TaskDto ConvertToTaskDto(Models.Task model)
      => new()
      {
        Id = model.Id.GuidToText(),
        Description = model.Description,
        Deadline = model.Deadline,
        Comments = model.Comments.Select(s => ConvertToCommentDto(s)),
        CreatedAt = model.CreatedAt,
        Priority = ConvertToTaskPriorityDto(model.Priority),
        Status = model.Status.Value.ToString(),
        Title = model.Title,
        SpentTime = ConvertSpentTime(model.SpentTime),
        AssignedPerson = ConvertToUserDto(userService.GetById(model.AssignedPersonId))
      };

    public TeamDto ConvertToTeamDto(Team model)
      => new()
      {
        Id = model.Id.GuidToText(),
        Name = model.Name,
        TeamLeader = ConvertToUserDto(userService.GetById(model.TeamLeaderId)),
        Users = ConvertUserIdsToUserDtos(model.UserIds)
      };

    public UserDto ConvertToUserDto(User model)
    {
      if (model != null)
      {
        return new UserDto
        {
          Id = model.Id.GuidToText(),
          Name = model.Name,
          Surname = model.Surname,
          Email = model.Email,
          PhoneNumber = model.PhoneNumber,
          Position = model.Position,
        };
      }

      return null;
    }

    public DataChangeDto ConvertToDataChangeDto(DataChange model)
      => new()
      {
        Id = model.Id.GuidToText(),
        ObjectId = model.ObjectId,
        ObjectType = model.ObjectType,
        OperationResult = model.OperationResult,
        OperationTime = model.OperationTime,
        OperationType = model.OperationType,
        ChangeDetails = ConvertToChangeDetailsDtoList(model.ChangeDetails)
      };

    public PageableResultDto<DataChangeDto> ConvertToPageableResultDtoDataChangeDto(PageableResult<DataChange> model)
      => new()
      {
        Items = model.Items.Select(s => ConvertToDataChangeDto(s)).ToArray(),
        Pagination = ConvertToPaginationDto(model.Pagination)
      };

    public PageableResultDto<TeamDto> ConvertToPageableResultDtoTeamDto(PageableResult<Team> model)
      => new()
      {
        Items = model.Items.Select(s => ConvertToTeamDto(s)).ToArray(),
        Pagination = ConvertToPaginationDto(model.Pagination)
      };


    public PageableResultDto<UserDto> ConvertToPageableResultDtoUserDto(PageableResult<User> model)
      => new()
      {
        Items = model.Items.Select(s => ConvertToUserDto(s)).ToArray(),
        Pagination = ConvertToPaginationDto(model.Pagination)
      };

    public PageableResultDto<TaskDto> ConvertToPageableResultDtoTaskDto(PageableResult<Models.Task> model)
      => new()
      {
        Items = model.Items.Select(s => ConvertToTaskDto(s)).ToArray(),
        Pagination = ConvertToPaginationDto(model.Pagination)
      };

    public PageableResultDto<ProjectDto> ConvertToPageableResultDtoProjectDto(PageableResult<Project> model)
      => new()
      {
        Items = model.Items.Select(s => ConvertToProjectDto(s)).ToArray(),
        Pagination = ConvertToPaginationDto(model.Pagination)
      };

    private List<ChangeDetailsDto> ConvertToChangeDetailsDtoList(List<ChangeDetails> changeDetails)
    {
      var changes = new List<ChangeDetailsDto>();

      foreach (var changeDetail in changeDetails)
      {
        var dto = ConvertToChangeDetail(changeDetail);
        changes.Add(dto);
      }

      return changes;
    }

    private ChangeDetailsDto ConvertToChangeDetail(ChangeDetails model)
    => new()
    {
      PropertyName = model.PropertyName,
      NewValue = model.NewValue,
      OldValue = model.OldValue,
    };

    private List<TaskDto> ConvertTaskIdsToTaskDtos(List<Guid> taskIds)
    {
      if (taskIds != null)
      {
        var taskDtoList = new List<TaskDto>();

        foreach (var taskId in taskIds)
        {
          var task = taskService.GetById(taskId);
          var taskDto = ConvertToTaskDto(task);
          taskDtoList.Add(taskDto);
        }

        return taskDtoList;
      }
      return null;
    }

    private List<UserDto> ConvertUserIdsToUserDtos(List<Guid> userIds)
    {
      if (userIds != null)
      {
        var userDtoList = new List<UserDto>();

        foreach (var userId in userIds)
        {
          var user = userService.GetById(userId);
          var userDto = ConvertToUserDto(user);
          userDtoList.Add(userDto);
        }
        return userDtoList;
      }
      return null;
    }

    private string ConvertSpentTime(int spentTime)
    {
      int hours = spentTime / 60;
      int minutes = spentTime % 60;

      return $"{hours} godzin, {minutes} minut";
    }

    private PaginationDto ConvertToPaginationDto(Pagination model)
      => new()
      {
        PageNumber = model.PageNumber,
        PageSize = model.PageSize,
        TotalElements = model.TotalElements,
      };

    private TaskPriorityDto ConvertToTaskPriorityDto(string priorityId)
    {
      var taskPriority = taskPriorityService.GetPriority(priorityId);

      return new()
      {
        Id = taskPriority.Id,
        Name = taskPriority.Name,
      };
    }
  }
}
