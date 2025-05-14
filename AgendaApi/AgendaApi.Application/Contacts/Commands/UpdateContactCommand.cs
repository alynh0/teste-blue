using MediatR;

namespace AgendaApi.Application.Contacts.Commands
{
    public record UpdateContactCommand(
        Guid Id,
        string Name,
        string Email,
        string Phone
        ) : IRequest<Unit>;
}
