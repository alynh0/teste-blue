using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Application.Contacts.Handlers.Commands;
using AgendaApi.Infra.Data;
using AgendaApi.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AgendaApi.Tests.UnitTests.Application.Handlers.Commands
{
    public class CreateContactCommandHandlerTests
    {
        private static AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Handle_Should_Create_Contact_When_Valid()
        {
            // Arrange
            await using var context = CreateInMemoryDbContext();
            var handler = new CreateContactCommandHandler(context);
            var command = MockData.GetValidCreateCommand();

            // Act
            var newId = await handler.Handle(command, CancellationToken.None);

            // Assert
            var created = await context.Contacts.FindAsync(newId);
            Assert.NotNull(created);
            Assert.Equal(command.Name, created.Name);
            Assert.Equal(command.Email, created.Email);
            Assert.Equal(command.Phone, created.Phone);
        }

        [Fact]
        public async Task Handle_Should_Throw_ValidationException_When_Email_Exists()
        {
            // Arrange
            await using var context = CreateInMemoryDbContext();
            var existing = MockData.GetContacts(1)[0];
            context.Contacts.Add(existing);
            await context.SaveChangesAsync();

            var handler = new CreateContactCommandHandler(context);
            var command = new CreateContactCommand(
                existing.Name,
                existing.Email,
                "(81) 99999-9999"
            );

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(
                () => handler.Handle(command, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_Should_Throw_ValidationException_When_Name_Exists()
        {
            // Arrange
            await using var context = CreateInMemoryDbContext();

            var existing = MockData.GetContacts(1)[0];
            context.Contacts.Add(existing);
            await context.SaveChangesAsync();

            var handler = new CreateContactCommandHandler(context);

            var command = new CreateContactCommand(
                Name: existing.Name,
                Email: "email-novo@teste.com",
                Phone: "(81) 99999-0000"
            );

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(
                () => handler.Handle(command, CancellationToken.None)
            );
            Assert.Equal("Não é possível criar dois contatos com o mesmo nome.", ex.Message);
        }
    }
}
