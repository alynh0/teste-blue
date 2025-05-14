using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Infra.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AgendaApi.Application.Contacts.Validators
{
    public class UpdateContactValidator : AbstractValidator<UpdateContactCommand>
    {
        private readonly AppDbContext _context;

        public UpdateContactValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Máximo de 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório")
                .EmailAddress().WithMessage("E-mail inválido")
                .MustAsync(BeUniqueEmail).WithMessage("E-mail já está em uso");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefone é obrigatório")
                .Matches(@"^\(\d{2}\) \d{4,5}-\d{4}$").WithMessage("Formato inválido");
        }

        private async Task<bool> BeUniqueEmail(UpdateContactCommand command, string email, CancellationToken ct)
        {
            return !await _context.Contacts
                .AnyAsync(c => c.Email == email && c.Id != command.Id, ct);
        }
    }
}

