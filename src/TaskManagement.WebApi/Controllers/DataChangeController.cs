
using Domain;
using Microsoft.AspNetCore.Mvc;
using Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Mime;
using WebApi.Converts;
using WebApi.Dtos;
using WebApi.Examples;
using WebApi.Extensions;
using WebApi.Queries;

namespace WebApi.Controllers
{
  [Route("api/dataChange")]
  [ApiController]
  [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanego zapisu o historii zmian w bazie danych.")]
  [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
  public class DataChangeController : ControllerBase
  {
    public const string GetDataChangeByIdOperationType = "Pobranie szczegółów historii zmian po identyfikatorze";
    public const string GetAllDataChangesByOperationType = "Pobranie historii zmian";

    private readonly IDataChangeService dataChangeService;
    private readonly IDtoBuilder dtoBuilder;

    public DataChangeController(IDataChangeService dataChangeService, IDtoBuilder dtoBuilder)
    {
      this.dataChangeService = dataChangeService;
      this.dtoBuilder = dtoBuilder;
    }

    /// <summary>
    /// Pobiera szczegóły historii zmian po identyfikatorze
    /// </summary>
    /// <param name="id">Identyfikator historii zmian</param>
    /// <returns>Historia zmian o podanym identyfikatorze</returns>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(DataChangeDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono pozycji dziennika zmian.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DataChangeDtoResponseExample))]
    [SwaggerOperation(GetDataChangeByIdOperationType)]
    public async Task<IActionResult> GetById(string id)
    {
      var result = await dataChangeService.GetByIdAsync(id.TextToGuid());

      return result == null
      ? NotFound()
      : Ok(dtoBuilder.ConvertToDataChangeDto(result));
    }

    /// <summary>
    /// Pobiera listę pełnej historii zmian wraz z paginacją
    /// </summary>
    /// <returns>Lista historii zmian z paginacją</returns>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PageableResultDto<DataChangeDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PageableResultDtoDataChangeResponseExample))]
    [SwaggerOperation(GetAllDataChangesByOperationType)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
      var sort = SortExtension.GetSortParts(query.Sort);
      var input = new PageableInput
      {
        PageNumber = query.PageNumber,
        PageSize = query.PageSize,
        Sorting = sort
      };

      var dataChanges = await dataChangeService.GetAllAsync(input);

      if (dataChanges == null)
      {
        return NotFound();
      }

      return Ok(dtoBuilder.ConvertToPageableResultDtoDataChangeDto(dataChanges));
    }
  }
}
