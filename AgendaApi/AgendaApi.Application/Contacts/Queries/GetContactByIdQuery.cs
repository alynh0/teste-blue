using AgendaApi.Application.Contacts.DTOs;
using MediatR;

namespace AgendaApi.Application.Contacts.Queries
{
    public record GetContactByIdQuery(Guid Id) : IRequest<ContactDto>;
}
