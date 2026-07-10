using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Milo.Application.Service.Commands.CreateService;
using Milo.Application.Service.Commands.DeleteService;
using Milo.Application.Service.Commands.UpdateService;
using Milo.Application.Service.Queries.GetServiceById;
using Milo.Application.Service.Queries.GetServices;

namespace Milo.Api.Controllers;

[ApiController]
[Route("api/services")]
public sealed class ServiceController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string search = "",
        [FromQuery] Guid providerId = default,
        [FromQuery] string sortBy = "",
        [FromQuery] bool descending = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetServicesQuery(search, providerId, sortBy, descending, page, pageSize),
            cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetServiceByIdQuery(id), cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status404NotFound);
    }

    [Authorize(Roles = "Provider")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateServiceCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id, [FromBody] UpdateServiceCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("El Id de la ruta no coincide con el del cuerpo");

        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status404NotFound);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteServiceCommand(id), cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : Problem(title: result.Error, statusCode: StatusCodes.Status404NotFound);
    }
}