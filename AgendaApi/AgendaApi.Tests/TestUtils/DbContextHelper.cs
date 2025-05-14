using AgendaApi.Domain.Entities;
using AgendaApi.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AgendaApi.Tests.TestUtils
{
    public static class DbContextHelper
    {
        public static (Mock<AppDbContext>, Mock<DbSet<Contact>>) CreateMockDbContext()
        {
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockContext = new Mock<AppDbContext>();

            mockContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);

            return (mockContext, mockDbSet);
        }
    }
}
