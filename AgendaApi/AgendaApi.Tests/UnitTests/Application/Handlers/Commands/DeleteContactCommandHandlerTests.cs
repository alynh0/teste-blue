using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Application.Contacts.Handlers.Commands;
using AgendaApi.Domain.Interfaces;
using MediatR;
using Moq;

namespace AgendaApi.Tests.UnitTests.Application.Handlers.Commands
{
    public class DeleteContactCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_DeleteAsync_And_Return_Unit()
        {
            // Arrange
            var repoMock = new Mock<IContactRepository>();
            var handler = new DeleteContactCommandHandler(repoMock.Object);
            var fakeId = Guid.NewGuid();
            var command = new DeleteContactCommand(fakeId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            repoMock.Verify(r => r.DeleteAsync(fakeId, CancellationToken.None), Times.Once);
            Assert.Equal(Unit.Value, result);
        }
    }
}
