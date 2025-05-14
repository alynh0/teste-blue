using MediatR;

namespace AgendaApi.Application.Contacts.Commands
{
    public record CreateContactCommand(
        string Name,
        string Email,
        string Phone
        ) : IRequest<Guid>;
}
