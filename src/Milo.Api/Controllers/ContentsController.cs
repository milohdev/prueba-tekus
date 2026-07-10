using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Milo.Application.Contents.Commands.CreateContent;
using Milo.Application.Contents.Commands.DeleteContent;
using Milo.Application.Contents.Commands.UpdateContent;
using Milo.Application.Contents.Queries.GetContentById;
using Milo.Application.Contents.Queries.GetContents;

namespace Milo.Api.Controllers;

[ApiController]
[Route("api/contents")]
public sealed class ContentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetContentsQuery(), cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetContentByIdQuery(id), cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status404NotFound);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateContentCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: result.Error, statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id, [FromBody] UpdateContentCommand command, CancellationToken cancellationToken)
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
        var result = await sender.Send(new DeleteContentCommand(id), cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : Problem(title: result.Error, statusCode: StatusCodes.Status404NotFound);
    }
}