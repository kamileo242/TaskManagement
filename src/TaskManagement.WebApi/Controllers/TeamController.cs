using Domain;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Converts;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using TaskManagement.WebApi.Controllers;
using WebApi.Dtos;
using WebApi.Examples;
using WebApi.Extensions;
using WebApi.Queries;

namespace WebApi.Controllers
{
  [Route("api/team")]
  [ApiController]
  [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanego zespołu w bazie danych.")]
  [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
  public class TeamController : BaseController
  {
    public const string GetTeamByIdOperationType = "Pobranie zespołu po identyfikatorze";
    public const string GetAllTeamsByOperationType = "Pobranie wszystkich zespołów";
    public const string PostTeamOperationType = "Dodanie zespołu";
    public const string DeleteTeamOperationType = "Usunięcie zespołu";
    public const string PostTeamLeaderOperationType = "Ustawienie Team Leadera zespołu";
    public const string PostUserToTeamOperationType = "Dodanie użytkownika do zespołu";
    public const string DeleteUserFromTeamOperationType = "Usunięcie użytkownika z zespołu";
    public const string PatchTeamOperationType = "Zmodyfikowanie zespołu";

    private readonly ITeamServiceWithHistory teamService;
    private readonly IDtoBuilder dtoBuilder;

    public TeamController(ITeamServiceWithHistory teamService, IDtoBuilder dtoBuilder)
    {
      this.teamService = teamService;
      this.dtoBuilder = dtoBuilder;
    }

    /// <summary>
    /// Pobiera zespół po identyfikatorze
    /// </summary>
    /// <param name="id">Identyfikator zespołu</param>
    /// <returns>Użytkownik o podanym identyfikatorze</returns>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zespołu.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TeamDtoResponseExample))]
    [SwaggerOperation(GetTeamByIdOperationType)]
    public async Task<IActionResult> GetById([Required] string id)
    {
      var result = await teamService.GetByIdAsync(id.TextToGuid());

      return result == null
        ? NotFound()
        : Ok(dtoBuilder.ConvertToTeamDto(result));
    }

    /// <summary>
    /// Pobiera listę wszystkich zespołów
    /// </summary>
    /// <returns>Lista zespołów</returns>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PageableResultDto<TeamDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PageableResultDtoTeamDtoResponseExample))]
    [SwaggerOperation(GetAllTeamsByOperationType)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
      var sort = SortExtension.GetSortParts(query.Sort);
      var input = new PageableInput
      {
        PageNumber = query.PageNumber,
        PageSize = query.PageSize,
        Sorting = sort
      };

      var teams = await teamService.GetAllAsync(input);

      return Ok(dtoBuilder.ConvertToPageableResultDtoTeamDto(teams));
    }

    /// <summary>
    /// Dodaje zespół
    /// </summary>
    /// <param name="teamDto">Dane zespołu</param>
    /// <returns>Nowy zespół</returns>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TeamDtoResponseExample))]
    [SwaggerRequestExample(typeof(StoreTeamDto), typeof(TeamDtoRequestExample))]
    [SwaggerOperation(PostTeamOperationType)]
    public async Task<IActionResult> Post([FromBody] StoreTeamDto teamDto)
    {
      var context = CreateContext(PostTeamOperationType);

      var result = await teamService.AddAsync(context, teamDto.ToModel());

      return Ok(dtoBuilder.ConvertToTeamDto(result));
    }

    /// <summary>
    /// Usuwa zespół
    /// </summary>
    /// <param name="id">Identyfikator zespołu</param>
    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(DeleteTeamOperationType)]
    public async Task<IActionResult> Delete([Required] string id)
    {
      var context = CreateContext(DeleteTeamOperationType);

      await teamService.DeleteAsync(context, id.TextToGuid());

      return NoContent();
    }

    /// <summary>
    /// Ustawia TeamLeadera zespołu
    /// </summary>
    /// <param name="teamId">Identyfikator zespołu</param>
    /// <param name="userId">Identyfikator użytkownika</param>
    /// <returns>Zmodyfikowany zespół</returns>
    [HttpPut]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zespołu.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TeamDtoResponseExample))]
    [SwaggerOperation(PostTeamLeaderOperationType)]
    public async Task<IActionResult> SetTeamLeader([Required] string teamId, [Required] string userId)
    {
      var context = CreateContext(PostTeamLeaderOperationType);

      var result = await teamService.AddTeamLeader(context, teamId.TextToGuid(), userId.TextToGuid());

      return result == null
        ? NotFound()
        : Ok(dtoBuilder.ConvertToTeamDto(result));
    }

    /// <summary>
    /// Dodaje użytkownika do zespołu
    /// </summary>
    /// <param name="teamId">Identyfikator zespołu</param>
    /// <param name="userId">Identyfikator użytkownika</param>
    /// <returns>Zmodyfikowany zespół</returns>
    [HttpPost("{userId}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zespołu.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TeamDtoResponseExample))]
    [SwaggerOperation(PostUserToTeamOperationType)]
    public async Task<IActionResult> AddUserToTeam([Required] string teamId, [Required] string userId)
    {
      var context = CreateContext(PostUserToTeamOperationType);

      var result = await teamService.AddUserToTeam(context, teamId.TextToGuid(), userId.TextToGuid());

      return result == null
        ? NotFound()
        : Ok(dtoBuilder.ConvertToTeamDto(result));
    }

    /// <summary>
    /// Usuwa użytkownika z zespołu
    /// </summary>
    /// <param name="teamId">Identyfikator zespołu</param>
    /// <param name="userId">Identyfikator użytkownika</param>
    /// <returns>Zmodyfikowany zespół</returns>
    [HttpDelete("user/{userId}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TeamDtoResponseExample))]
    [SwaggerOperation(DeleteUserFromTeamOperationType)]
    public async Task<IActionResult> DeleteUserFromTeam([Required] string teamId, [Required] string userId)
    {
      var context = CreateContext(DeleteUserFromTeamOperationType);

      var result = await teamService.DeleteUserFromTeam(context, teamId.TextToGuid(), userId.TextToGuid());

      return result == null
        ? NotFound()
        : Ok(dtoBuilder.ConvertToTeamDto(result));
    }

    /// <summary>
    /// Modyfikuje zespół
    /// </summary>
    /// <param name="id">Identyfikator zespołu</param>
    [HttpPatch("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono zespołu.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TeamDtoResponseExample))]
    [SwaggerRequestExample(typeof(PatchTeamDto), typeof(ChangeDtoTeamDtoRequestExample))]
    [SwaggerOperation(PatchTeamOperationType)]
    public async Task<IActionResult> Patch([Required] string id, [FromBody] ChangeDto<PatchTeamDto> changeTeam)
    {
      var context = CreateContext(PatchTeamOperationType);

      var result = await teamService.PatchAsync(context, id.TextToGuid(), changeTeam.ToModel());

      return result == null
        ? NotFound()
        : Ok(dtoBuilder.ConvertToTeamDto(result));
    }
  }
}
