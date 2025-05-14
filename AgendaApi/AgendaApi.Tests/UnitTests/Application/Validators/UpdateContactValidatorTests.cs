using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Application.Contacts.Validators;
using AgendaApi.Domain.Entities;
using AgendaApi.Infra.Data;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace AgendaApi.Tests.UnitTests.Application.Validators
{
    public class UpdateContactValidatorTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly UpdateContactValidator _validator;

        public UpdateContactValidatorTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _context.Contacts.Add(new Contact
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "Existente",
                Email = "existe@teste.com",
                Phone = "(11) 90000-0000",
                CreatedAt = DateTime.UtcNow
            });
            _context.SaveChanges();

            _validator = new UpdateContactValidator(_context);
        }

        [Fact]
        public async Task Should_Have_Error_When_Id_Is_Empty()
        {
            var cmd = new UpdateContactCommand(Guid.Empty, "N", "e@t.com", "(11) 91234-5678");
            var result = await _validator.TestValidateAsync(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public async Task Should_Have_Error_When_Name_Is_Empty()
        {
            var cmd = new UpdateContactCommand(Guid.NewGuid(), "", "e@t.com", "(11) 91234-5678");
            var result = await _validator.TestValidateAsync(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Nome é obrigatório");
        }

        [Fact]
        public async Task Should_Have_Error_When_Name_Too_Long()
        {
            var longName = new string('B', 101);
            var cmd = new UpdateContactCommand(Guid.NewGuid(), longName, "e@t.com", "(11) 91234-5678");
            var result = await _validator.TestValidateAsync(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Máximo de 100 caracteres");
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Empty()
        {
            var cmd = new UpdateContactCommand(Guid.NewGuid(), "Nome", "", "(11) 91234-5678");
            var result = await _validator.TestValidateAsync(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("E-mail é obrigatório");
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Invalid()
        {
            var cmd = new UpdateContactCommand(Guid.NewGuid(), "Nome", "invalido", "(11) 91234-5678");
            var result = await _validator.TestValidateAsync(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("E-mail inválido");
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Already_In_Use()
        {
            var cmd = new UpdateContactCommand(
                Id: Guid.NewGuid(),
                Name: "Outro",
                Email: "existe@teste.com",
                Phone: "(11) 91234-5678"
            );
            var result = await _validator.TestValidateAsync(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("E-mail já está em uso");
        }

        [Fact]
        public async Task Should_Have_Error_When_Phone_Is_Empty()
        {
            var cmd = new UpdateContactCommand(Guid.NewGuid(), "Nome", "e@t.com", "");
            var result = await _validator.TestValidateAsync(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Phone)
                  .WithErrorMessage("Telefone é obrigatório");
        }

        [Fact]
        public async Task Should_Have_Error_When_Phone_Invalid_Format()
        {
            var cmd = new UpdateContactCommand(Guid.NewGuid(), "Nome", "e@t.com", "12345");
            var result = await _validator.TestValidateAsync(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Phone)
                  .WithErrorMessage("Formato inválido");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var existingId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var cmd = new UpdateContactCommand(existingId, "Novo Nome", "novo@teste.com", "(11) 92345-6789");
            var result = await _validator.TestValidateAsync(cmd);
            result.ShouldNotHaveAnyValidationErrors();
        }

        public void Dispose() => _context.Dispose();
    }
}