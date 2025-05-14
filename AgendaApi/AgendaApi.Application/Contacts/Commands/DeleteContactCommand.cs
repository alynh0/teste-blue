using MediatR;

namespace AgendaApi.Application.Contacts.Commands
{
    public record DeleteContactCommand (Guid Id) : IRequest<Unit>;
}
