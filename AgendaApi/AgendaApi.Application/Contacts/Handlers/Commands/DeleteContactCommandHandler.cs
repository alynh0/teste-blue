using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Domain.Interfaces;
using MediatR;

namespace AgendaApi.Application.Contacts.Handlers.Commands
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Unit>
    {
        private readonly IContactRepository _repository;

        public DeleteContactCommandHandler(IContactRepository repository) => _repository = repository;

        public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken ct)
        {
            await _repository.DeleteAsync(request.Id, ct);
            return Unit.Value;
        }

    }
}
