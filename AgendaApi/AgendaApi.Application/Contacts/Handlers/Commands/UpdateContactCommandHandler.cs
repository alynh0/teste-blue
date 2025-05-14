using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AgendaApi.Application.Contacts.Handlers.Commands
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Unit>
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;

        public UpdateContactCommandHandler(IContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateContactCommand request, CancellationToken ct)
        {
            var contact = await _repository.GetByIdAsync(request.Id, ct);
            if (contact == null)
                throw new KeyNotFoundException("Contato não encontrado.");

            _mapper.Map(request, contact);
            contact.CreatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(contact, ct);
            return Unit.Value;
        }
    }
}
