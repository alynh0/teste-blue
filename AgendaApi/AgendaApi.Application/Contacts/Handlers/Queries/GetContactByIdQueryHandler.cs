using AgendaApi.Application.Contacts.DTOs;
using AgendaApi.Application.Contacts.Queries;
using AgendaApi.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AgendaApi.Application.Contacts.Handlers.Queries
{
    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto>
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;

        public GetContactByIdQueryHandler(IContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ContactDto> Handle(GetContactByIdQuery request, CancellationToken ct)
        {
            var contact = await _repository.GetByIdAsync(request.Id, ct);
            return _mapper.Map<ContactDto>(contact);
        }
    }
}
