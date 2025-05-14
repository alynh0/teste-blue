using AgendaApi.Application.Contacts.DTOs;
using AgendaApi.Application.Contacts.Queries;
using AgendaApi.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AgendaApi.Application.Contacts.Handlers.Queries
{
    public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, List<ContactDto>>
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;

        public GetAllContactsQueryHandler(IContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ContactDto>> Handle(GetAllContactsQuery request, CancellationToken ct)
        {
            var contacts = await _repository.GetAllAsync(ct);
            return _mapper.Map<List<ContactDto>>(contacts);
        }
    }
}
