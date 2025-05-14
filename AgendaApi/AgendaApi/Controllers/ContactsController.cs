using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Application.Contacts.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContactsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
    [FromBody] CreateContactCommand command,
    [FromServices] IValidator<CreateContactCommand> validator)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetContactByIdQuery(id);
        var contact = await _mediator.Send(query, ct);
        return contact != null ? Ok(contact) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var query = new GetAllContactsQuery();
        var contacts = await _mediator.Send(query, ct);
        return Ok(contacts);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteContactCommand(id), ct);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid Id,
        [FromBody] UpdateContactCommand command,
        [FromServices] IValidator<UpdateContactCommand> validator,
        CancellationToken ct)
    {
        if (Id != command.Id)
            return BadRequest("ID da rota não corresponde ao ID do corpo");

        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        await _mediator.Send(command, ct);
        return NoContent();
    }
}