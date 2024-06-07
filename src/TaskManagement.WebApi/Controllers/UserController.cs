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
  [Route("api/user")]
  [ApiController]
  [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanego użytkownika w bazie danych.")]
  [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
  public class UserController : ControllerBase
  {
    public const string GetUserByIdOperationType = "Pobranie użytkownika po identyfikatorze";
    public const string GetAllUsersByOperationType = "Pobranie wszystkich użytkowników";
    public const string PostUserOperationType = "Dodanie użytkownika";
    public const string DeleteUserOperationType = "Usunięcie użytkownika";
    public const string PatchUserOperationType = "Zmodyfikowanie użytkownika";

    private readonly IUserService userService;
    private readonly IDtoBuilder dtoBuilder;

    public UserController(IUserService userService, IDtoBuilder dtoBuilder)
    {
      this.userService = userService;
      this.dtoBuilder = dtoBuilder;
    }

    /// <summary>
    /// Pobiera użytkownika po identyfikatorze
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    /// <returns>Użytkownik o podanym identyfikatorze</returns>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono użytkownika.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserDtoResponseExample))]
    [SwaggerOperation(GetUserByIdOperationType)]
    public async Task<IActionResult> GetById([Required] string id)
    {
      var result = await userService.GetByIdAsync(id.TextToGuid());

      return result == null
        ? NotFound()
        : Ok(dtoBuilder.ConvertToUserDto(result));
    }

    /// <summary>
    /// Pobiera listę wszystkich użytkowników
    /// </summary>
    /// <returns>Lista użytkowników </returns>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PageableResultDto<UserDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PageableResultDtoUserDtoResponseExample))]
    [SwaggerOperation(GetAllUsersByOperationType)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
      var sort = SortExtension.GetSortParts(query.Sort);
      var input = new PageableInput
      {
        PageNumber = query.PageNumber,
        PageSize = query.PageSize,
        Sorting = sort
      };

      var users = await userService.GetAllAsync(input);

      return Ok(dtoBuilder.ConvertToPageableResultDtoUserDto(users));
    }

    /// <summary>
    /// Dodaje użytkownika
    /// </summary>
    /// <param name="userDto">Dane użytkownika</param>
    /// <returns>Nowy użytkownik</returns>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserDtoResponseExample))]
    [SwaggerRequestExample(typeof(StoreUserDto), typeof(UserDtoRequestExample))]
    [SwaggerOperation(PostUserOperationType)]
    public async Task<IActionResult> Post([FromBody] StoreUserDto userDto)
    {
      var user = userDto.ToModel();

      var result = await userService.AddAsync(user);

      return Ok(dtoBuilder.ConvertToUserDto(result));
    }

    /// <summary>
    /// Usuwa użytkownika
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerOperation(DeleteUserOperationType)]
    public async Task<IActionResult> Delete([Required] string id)
    {
      await userService.DeleteAsync(id.TextToGuid());

      return NoContent();
    }

    /// <summary>
    /// Modyfikuje użytkownika
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    [HttpPatch("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono użytkownika.")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserDtoResponseExample))]
    [SwaggerRequestExample(typeof(PatchUserDto), typeof(ChangeDtoUserDtoRequestExample))]
    [SwaggerOperation(PatchUserOperationType)]
    public async Task<IActionResult> Patch([Required] string id, [FromBody] ChangeDto<PatchUserDto> changeUser)
    {
      var result = await userService.Patch(id.TextToGuid(), changeUser.ToModel());

      return result == null
        ? NotFound()
        : Ok(dtoBuilder.ConvertToUserDto(result));
    }
  }
}
