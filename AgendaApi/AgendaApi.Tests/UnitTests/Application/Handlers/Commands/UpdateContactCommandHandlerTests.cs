using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Application.Contacts.Handlers.Commands;
using AgendaApi.Domain.Entities;
using AgendaApi.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Moq;

namespace AgendaApi.Tests.UnitTests.Application.Handlers.Commands
{
    public class UpdateContactCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Throw_KeyNotFoundException_When_Contact_Not_Found()
        {
            // Arrange
            var repoMock = new Mock<IContactRepository>();
            repoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync((Contact?)null);

            var mapperMock = new Mock<IMapper>();
            var handler = new UpdateContactCommandHandler(repoMock.Object, mapperMock.Object);
            var cmd = new UpdateContactCommand(Guid.NewGuid(), "N", "e@t.com", "123");

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => handler.Handle(cmd, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_Should_Map_Update_And_Call_UpdateAsync()
        {
            // Arrange
            var existing = new Contact
            {
                Id = Guid.NewGuid(),
                Name = "Old",
                Email = "old@t.com",
                Phone = "000",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            };

            var repoMock = new Mock<IContactRepository>();
            repoMock
                .Setup(r => r.GetByIdAsync(existing.Id, CancellationToken.None))
                .ReturnsAsync(existing);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map(It.IsAny<UpdateContactCommand>(), It.IsAny<Contact>()))
                .Callback<UpdateContactCommand, Contact>((cmd, ct) =>
                {
                    ct.Name = cmd.Name;
                    ct.Email = cmd.Email;
                    ct.Phone = cmd.Phone;
                });

            var handler = new UpdateContactCommandHandler(repoMock.Object, mapperMock.Object);
            var cmd = new UpdateContactCommand(existing.Id, "NewName", "new@t.com", "999");

            var before = DateTime.UtcNow;

            // Act
            var result = await handler.Handle(cmd, CancellationToken.None);

            // Assert
            mapperMock.Verify(m => m.Map(cmd, existing), Times.Once);

            Assert.True(existing.CreatedAt >= before,
                "CreatedAt deveria ter sido sobrescrito após o mapeamento");

            repoMock.Verify(r => r.UpdateAsync(
                It.Is<Contact>(c =>
                    c.Id == existing.Id
                 && c.Name == cmd.Name
                 && c.Email == cmd.Email
                 && c.Phone == cmd.Phone
                ),
                CancellationToken.None
            ), Times.Once);

            Assert.Equal(Unit.Value, result);
        }
    }
}
