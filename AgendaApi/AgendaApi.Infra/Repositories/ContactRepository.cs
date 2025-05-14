using AgendaApi.Domain.Entities;
using AgendaApi.Domain.Interfaces;
using AgendaApi.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AgendaApi.Infra.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;

        public ContactRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Guid> AddAsync(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return contact.Id;
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var contact = new Contact { Id = id };
            _context.Contacts.Attach(contact);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<List<Contact>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Contacts.ToListAsync(ct);
        }

        public async Task<Contact?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Contacts.FindAsync(id, ct);
        }

        public async Task UpdateAsync(Contact contact, CancellationToken ct)
        {
            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync(ct);
        }
    }
}
