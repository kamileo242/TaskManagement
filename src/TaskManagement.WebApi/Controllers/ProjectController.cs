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
  [Route("api/project")]
  [ApiController]
  [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanego projektu w bazie danych.")]
  [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
  public class ProjectController : ControllerBase
  {
    public const string GetProjectByIdOperationType = "Pobranie projektu po identyfikatorze";
    public const string GetAllProjectsByOperationType = "Pobranie wszystkich projektów";
    public const string PostProjectOperationType = "Dodanie projektu";
    public const string DeleteProjectOperationType = "Usunięcie projektu";
    public const string PostCommentOperationType = "Dodanie komentarza do projektu";
    public const string DeleteCommentOperationType = "Usunięcie komentarza z projektu";
    public const string EndProjectOperationType = "Zakonczenie projektu";
    public const string DeleteTaskFromProjectOperationType = "Usunięcie zadania z projektu";
    public const string PatchProjectOperationType = "Zmodyfikowanie projektu";

    private readonly IProjectService projectService;
    private readonly IDtoBuilder dtoBuilder;

    public ProjectController(IProjectService projectService, IDtoBuilder dtoBuilder)
    {
      this.projectService = projectService;
      this.dtoBuilder = dtoBuilder;
    }

    /// <summary>
    /// Pobiera projekt po identyfikatorze
    /// </summary>
    /// <param name="id">Identyfikator projektu</param>
    /// <returns>Projekt o podanym identyfikatorze</returns>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono projektu.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProjectDtoResponseExample))]
    [SwaggerOperation(GetProjectByIdOperationType)]
    public async Task<IActionResult> GetById([Required] string id)
    {
      var result = await projectService.GetByIdAsync(id.TextToGuid());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToProjectDto(result));
    }

    /// <summary>
    /// Pobiera listę wszystkich projektów
    /// </summary>
    /// <returns>Lista projektów</returns>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PageableResultDto<ProjectDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PageableResultDtoProjectDtoResponseExample))]
    [SwaggerOperation(GetAllProjectsByOperationType)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
      var sort = SortExtension.GetSortParts(query.Sort);
      var input = new PageableInput
      {
        PageNumber = query.PageNumber,
        PageSize = query.PageSize,
        Sorting = sort
      };

      var result = await projectService.GetAllAsync(input);

      return Ok(dtoBuilder.ConvertToPageableResultDtoProjectDto(result));
    }

    /// <summary>
    /// Dodaje nowy projekt
    /// </summary>
    /// <param name="projectDto">Dane projektu</param>
    /// <returns>Nowy projekt</returns>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProjectDtoResponseExample))]
    [SwaggerRequestExample(typeof(StoreProjectDto), typeof(ProjectDtoRequestExample))]
    [SwaggerOperation(PostProjectOperationType)]
    public async Task<IActionResult> Post([FromBody] StoreProjectDto projectDto)
    {
      var result = await projectService.AddAsync(projectDto.ToModel());

      return Ok(dtoBuilder.ConvertToProjectDto(result));
    }

    /// <summary>
    /// Usuwa projekt
    /// </summary>
    /// <param name="id">Identyfikator projektu</param>
    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(DeleteProjectOperationType)]
    public async Task<IActionResult> Delete([Required] string id)
    {
      await projectService.DeleteAsync(id.TextToGuid());

      return NoContent();
    }

    /// <summary>
    /// Zmienia status projektu
    /// </summary>
    /// <param name="projectId">Identyfikator projektu</param>
    /// <returns>Zmodyfikowany projekt</returns>
    [HttpPut]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono projektu.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProjectDtoResponseExample))]
    [SwaggerOperation(EndProjectOperationType)]
    public async Task<IActionResult> EndProject([Required] string projectId)
    {
      var result = await projectService.EndProjectAsync(projectId.TextToGuid());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToProjectDto(result));
    }

    /// <summary>
    /// Dodaje komentarz do projektu
    /// </summary>
    /// <param name="id">Identyfikator projektu</param>
    /// <returns>Zmodyfikowany projekt</returns>
    [HttpPost("comment")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono projektu.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProjectDtoResponseExample))]
    [SwaggerRequestExample(typeof(StoreCommentDto), typeof(CommentDtoRequestExample))]
    [SwaggerOperation(PostCommentOperationType)]
    public async Task<IActionResult> AddCommentToProject([Required] string id, [FromBody] StoreCommentDto commentDto)
    {
      var result = await projectService.AddCommentAsync(id.TextToGuid(), commentDto.ToModel());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToProjectDto(result));
    }

    /// <summary>
    /// Usuwa komentarz z projektu
    /// </summary>
    /// <param name="projectId">Identyfikator projektu</param>
    /// <param name="commentId">Identyfikator komentarza</param>
    /// <returns>Zmodyfikowany projekt</returns>
    [HttpDelete("comment")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono projektu.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProjectDtoResponseExample))]
    [SwaggerOperation(DeleteCommentOperationType)]
    public async Task<IActionResult> DeleteCommentFromProject([Required] string projectId, [Required] string commentId)
    {
      var result = await projectService.DeleteCommentAsync(projectId.TextToGuid(), commentId.TextToGuid());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToProjectDto(result));
    }

    /// <summary>
    /// Usuwa zadanie z projektu
    /// </summary>
    /// <param name="projectId">Identyfikator projektu</param>
    /// <param name="taskId">Identyfikator zadania</param>
    /// <returns>Zmodyfikowany projekt</returns>
    [HttpDelete("task")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono projektu.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProjectDtoResponseExample))]
    [SwaggerOperation(DeleteTaskFromProjectOperationType)]
    public async Task<IActionResult> DeleteTaskFromProject([Required] string projectId, [Required] string taskId)
    {
      var result = await projectService.DeleteTaskAsync(projectId.TextToGuid(), taskId.TextToGuid());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToProjectDto(result));
    }

    /// <summary>
    /// Modyfikuje projekt
    /// </summary>
    /// <param name="id">Identyfikator projektu</param>
    [HttpPatch("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono projektu.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProjectDtoResponseExample))]
    [SwaggerRequestExample(typeof(PatchProjectDto), typeof(ChangeDtoProjectDtoRequestExample))]
    [SwaggerOperation(PatchProjectOperationType)]
    public async Task<IActionResult> Patch([Required] string id, [FromBody] ChangeDto<PatchProjectDto> changeProject)
    {

      var result = await projectService.Patch(id.TextToGuid(), changeProject.ToModel());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToProjectDto(result));
    }
  }
}
