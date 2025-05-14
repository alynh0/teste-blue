﻿using AgendaApi.Application.Contacts.Commands;
using FluentValidation;

namespace AgendaApi.Application.Contacts.Validators
{
    public class CreateContactValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .MaximumLength(100).WithMessage("O nome deve conter no máximo 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório")
                .EmailAddress().WithMessage("o e-mail é inválido");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("O número do telefone é obrigatório")
                .Matches(@"^\(\d{2}\) \d{4,5}-\d{4}$").WithMessage("Formato inválido (ex: (81) 91111-1111)");
        }
    }
}
