using AgendaApi.Application.Contacts.Handlers.Queries;
using AgendaApi.Application.Contacts.Queries;
using AgendaApi.Application.Mapping;
using AgendaApi.Domain.Entities;
using AgendaApi.Domain.Interfaces;
using AutoMapper;
using Moq;

namespace AgendaApi.Tests.UnitTests.Application.Handlers.Queries
{
    public class GetContactByIdQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IContactRepository> _repoMock;
        private readonly GetContactByIdQueryHandler _handler;

        public GetContactByIdQueryHandlerTests()
        {
            var cfg = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
            _mapper = cfg.CreateMapper();

            _repoMock = new Mock<IContactRepository>();
            _handler = new GetContactByIdQueryHandler(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Contact_Not_Found()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Contact)null);

            // Act
            var result = await _handler.Handle(new GetContactByIdQuery(Guid.NewGuid()), CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_Should_Return_Mapped_Dto_When_Contact_Found()
        {
            // Arrange
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Name = "X",
                Email = "x@y.com",
                Phone = "222",
                CreatedAt = DateTime.UtcNow
            };
            _repoMock.Setup(r => r.GetByIdAsync(contact.Id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(contact);

            // Act
            var result = await _handler.Handle(new GetContactByIdQuery(contact.Id), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contact.Id, result.Id);
            Assert.Equal(contact.Name, result.Name);
            Assert.Equal(contact.Email, result.Email);
            Assert.Equal(contact.Phone, result.Phone);
        }
    }
}
