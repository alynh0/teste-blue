using AgendaApi.Application.Contacts.Handlers.Queries;
using AgendaApi.Application.Contacts.Queries;
using AgendaApi.Application.Mapping;
using AgendaApi.Domain.Entities;
using AgendaApi.Domain.Interfaces;
using AutoMapper;
using Moq;

namespace AgendaApi.Tests.UnitTests.Application.Handlers.Queries
{
    public class GetAllContactsQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IContactRepository> _repoMock;
        private readonly GetAllContactsQueryHandler _handler;

        public GetAllContactsQueryHandlerTests()
        {
            var cfg = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
            _mapper = cfg.CreateMapper();

            _repoMock = new Mock<IContactRepository>();
            _handler = new GetAllContactsQueryHandler(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Contacts()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new List<Contact>());

            // Act
            var result = await _handler.Handle(new GetAllContactsQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_Should_Return_Mapped_List_When_Contacts_Exist()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { Id = Guid.NewGuid(), Name = "A", Email = "a@x.com", Phone = "000", CreatedAt = DateTime.UtcNow },
                new Contact { Id = Guid.NewGuid(), Name = "B", Email = "b@x.com", Phone = "111", CreatedAt = DateTime.UtcNow }
            };
            _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(contacts);

            // Act
            var result = await _handler.Handle(new GetAllContactsQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, dto => dto.Id == contacts[0].Id && dto.Name == "A" && dto.Email == "a@x.com" && dto.Phone == "000");
            Assert.Contains(result, dto => dto.Id == contacts[1].Id && dto.Name == "B" && dto.Email == "b@x.com" && dto.Phone == "111");
        }
    }
}
