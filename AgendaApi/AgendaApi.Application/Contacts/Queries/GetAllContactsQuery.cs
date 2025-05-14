using AgendaApi.Application.Contacts.DTOs;
using MediatR;

namespace AgendaApi.Application.Contacts.Queries
{
    public record GetAllContactsQuery : IRequest<List<ContactDto>>;
}
