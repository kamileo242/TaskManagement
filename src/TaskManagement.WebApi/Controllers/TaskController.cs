using Domain;
using Microsoft.AspNetCore.Mvc;
using Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using WebApi.Converts;
using WebApi.Dtos;
using WebApi.Examples;
using WebApi.Extensions;
using WebApi.Queries;

namespace WebApi.Controllers
{
  [Route("api/task")]
  [ApiController]
  [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanego zadania w bazie danych.")]
  [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
  public class TaskController : Controller
  {
    public const string GetTaskByIdOperationType = "Pobranie zadania po identyfikatorze";
    public const string GetAllTasksByOperationType = "Pobranie wszystkich zadan";
    public const string PostTaskOperationType = "Dodanie zadania";
    public const string DeleteTaskOperationType = "Usunięcie zadania";
    public const string PostCommentOperationType = "Dodanie komentarza do zadania";
    public const string DeleteCommentOperationType = "Usunięcie komentarza z zadania";
    public const string EndTaskOperationType = "Zakonczenie zadania";
    public const string SetUserToTaskOperationType = "Przypisanie osoby do zadania";
    public const string SetTimeSpanToTaskOperationType = "Zarejestrowanie czasu pracy";
    public const string PatchTaskOperationType = "Zmodyfikowanie zadania";

    private readonly ITaskService taskService;
    private readonly IDtoBuilder dtoBuilder;
    public TaskController(ITaskService taskService, IDtoBuilder dtoBuilder)
    {
      this.taskService = taskService;
      this.dtoBuilder = dtoBuilder;
    }

    /// <summary>
    /// Pobiera zadanie po identyfikatorze
    /// </summary>
    /// <param name="id">Identyfikator zadania</param>
    /// <returns>Zadanie o podanym identyfikatorze</returns>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zadania.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TaskDtoResponseExample))]
    [SwaggerOperation(GetTaskByIdOperationType)]
    public async Task<IActionResult> GetById([Required] string id)
    {
      var result = await taskService.GetByIdAsync(id.TextToGuid());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToTaskDto(result));
    }

    /// <summary>
    /// Pobiera listę wszystkich zadań
    /// </summary>
    /// <returns>Lista zadań</returns>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PageableResultDto<TaskDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PageableResultDtoTaskDtoResponseExample))]
    [SwaggerOperation(GetAllTasksByOperationType)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
      var sort = SortExtension.GetSortParts(query.Sort);
      var input = new PageableInput
      {
        PageNumber = query.PageNumber,
        PageSize = query.PageSize,
        Sorting = sort
      };

      var tasks = await taskService.GetAllAsync(input);

      return Ok(dtoBuilder.ConvertToPageableResultDtoTaskDto(tasks));
    }

    /// <summary>
    /// Dodaje nowy zadanie
    /// </summary>
    /// <param name="projectId">Identyfikator projektu</param>
    /// <param name="taskDto">Dane zadania</param>
    /// <returns>Nowe zadanie</returns>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TaskDtoResponseExample))]
    [SwaggerRequestExample(typeof(StoreTaskDto), typeof(TaskDtoRequestExample))]
    [SwaggerOperation(PostTaskOperationType)]
    public async Task<IActionResult> Post([Required] string projectId, [FromBody] StoreTaskDto taskDto)
    {
      var result = await taskService.AddAsync(projectId.TextToGuid(), taskDto.ToModel());

      return Ok(dtoBuilder.ConvertToTaskDto(result));
    }

    /// <summary>
    /// Usuwa zadanie
    /// </summary>
    /// <param name="id">Identyfikator zadania</param>
    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(DeleteTaskOperationType)]
    public async Task<IActionResult> Delete([Required] string id)
    {
      await taskService.DeleteAsync(id.TextToGuid());

      return NoContent();
    }

    /// <summary>
    /// Zmienia status zadania
    /// </summary>
    /// <param name="projectId">Identyfikator zadania</param>
    /// <returns>Zmodyfikowane zadanie</returns>
    [HttpPut]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zadania.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TaskDtoResponseExample))]
    [SwaggerOperation(EndTaskOperationType)]
    public async Task<IActionResult> EndProject([Required] string projectId)
    {
      var result = await taskService.EndTaskStatusAsync(projectId.TextToGuid());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToTaskDto(result));
    }

    /// <summary>
    /// Dodaje komentarz do zadania
    /// </summary>
    /// <param name="id">Identyfikator zadania</param>
    /// <returns>Zmodyfikowane zadanie</returns>
    [HttpPost("comment")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zadania.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TaskDtoResponseExample))]
    [SwaggerRequestExample(typeof(StoreCommentDto), typeof(CommentDtoRequestExample))]
    [SwaggerOperation(PostCommentOperationType)]
    public async Task<IActionResult> AddCommentToTask([Required] string id, [FromBody] StoreCommentDto commentDto)
    {
      var result = await taskService.AddCommentAsync(id.TextToGuid(), commentDto.ToModel());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToTaskDto(result));
    }

    /// <summary>
    /// Usuwa komentarz z zadania
    /// </summary>
    /// <param name="taskId">Identyfikator zadania</param>
    /// <param name="commentId">Identyfikator komentarza</param>
    /// <returns>Zmodyfikowane zadanie</returns>
    [HttpDelete("comment")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zadania.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TaskDtoResponseExample))]
    [SwaggerOperation(DeleteCommentOperationType)]
    public async Task<IActionResult> DeleteCommentFromProject([Required] string taskId, [Required] string commentId)
    {
      var result = await taskService.DeleteCommentAsync(taskId.TextToGuid(), commentId.TextToGuid());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToTaskDto(result));
    }

    /// <summary>
    /// Przypisuje osobę do zadania
    /// </summary>
    /// <param name="projectId">Identyfikator zadania</param>
    /// <param name="userId">Identyfikator użytkownika</param>
    /// <returns>Zmodyfikowane zadanie</returns>
    [HttpPut("user")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zadania.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TaskDtoResponseExample))]
    [SwaggerOperation(SetUserToTaskOperationType)]
    public async Task<IActionResult> AssignPersonToTask([Required] string projectId, [Required] string userId)
    {
      var result = await taskService.AssignPersonToTaskAsync(projectId.TextToGuid(), userId.TextToGuid());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToTaskDto(result));
    }

    /// <summary>
    /// Rejestruje czas spędzony przy zadaniu
    /// </summary>
    /// <param name="projectId">Identyfikator zadania</param>
    /// <param name="timeInMinutes">Czas pracy w minutach</param>
    /// <returns>Zmodyfikowane zadanie</returns>
    [HttpPut("spentTime")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zadania.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TaskDtoResponseExample))]
    [SwaggerOperation(SetTimeSpanToTaskOperationType)]
    public async Task<IActionResult> RegisterTime([Required] string projectId, int timeInMinutes)
    {
      var result = await taskService.RegisterTimeAsync(projectId.TextToGuid(), timeInMinutes);

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToTaskDto(result));
    }

    /// <summary>
    /// Modyfikuje zadanie
    /// </summary>
    /// <param name="id">Identyfikator zadania</param>
    [HttpPatch("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zadania.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TaskDtoResponseExample))]
    [SwaggerRequestExample(typeof(PatchTaskDto), typeof(PageableResultDtoTaskDtoResponseExample))]
    [SwaggerOperation(PatchTaskOperationType)]
    public async Task<IActionResult> Patch([Required] string id, [FromBody] ChangeDto<PatchTaskDto> changeTask)
    {
      var result = await taskService.Patch(id.TextToGuid(), changeTask.ToModel());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToTaskDto(result));
    }
  }
}
