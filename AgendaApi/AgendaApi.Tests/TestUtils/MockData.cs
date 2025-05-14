using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Domain.Entities;

namespace AgendaApi.Tests.TestUtils
{
    public static class MockData
    {
        public static List<Contact> GetContacts(int count =3)
        {
            return Enumerable.Range(1, count).Select(i => new Contact
            {
                Id = Guid.NewGuid(),
                Name = $"Contato {i}",
                Email = $"contato{i}@teste.com",
                Phone = "(81) 99999-9999",
                CreatedAt = DateTime.UtcNow,
            }).ToList();
        }

        public static CreateContactCommand GetValidCreateCommand() => new("Alysson", "ajsouzapC@gmail.com", "(81) 99999-9999");
    }
}
