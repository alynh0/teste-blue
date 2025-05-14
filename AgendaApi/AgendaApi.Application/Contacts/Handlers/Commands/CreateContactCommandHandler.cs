using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Domain.Entities;
using AgendaApi.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AgendaApi.Application.Contacts.Handlers.Commands
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Guid>
    {
        private readonly AppDbContext _context;

        public CreateContactCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateContactCommand request, CancellationToken ct)
        {
            if (await _context.Contacts.AnyAsync(c => c.Email == request.Email, ct))
                throw new ValidationException("E-mail já cadastrado.");

            if (await _context.Contacts.AnyAsync(c => c.Name == request.Name, ct))
                throw new ValidationException("Não é possível criar dois contatos com o mesmo nome.");

            var contact = new Contact
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone
            };


            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync(ct);
            return contact.Id;
        }
    }
}
