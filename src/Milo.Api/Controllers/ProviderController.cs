using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Milo.Application.Provider.Commands.CreateProvider;
using Milo.Application.Provider.Commands.LoginProvider;
using Milo.Application.Provider.Commands.UpdateProvider;
using Milo.Application.Provider.Queries.GetProviderById;
using Milo.Application.Provider.Queries.GetProviders;

namespace Milo.Api.Controllers;

[ApiController]
[Route("api/providers")]
public sealed class ProviderController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string search = "",
        [FromQuery] string sortBy = "",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetProvidersQuery(search, sortBy, page, pageSize), cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProviderByIdQuery(id), cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status404NotFound);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProviderCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status400BadRequest);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginProviderCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status401Unauthorized);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id, [FromBody] UpdateProviderCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("El Id de la ruta no coincide con el del cuerpo");

        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status404NotFound);
    }
}