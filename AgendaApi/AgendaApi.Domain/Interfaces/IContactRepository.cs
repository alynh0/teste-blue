using AgendaApi.Domain.Entities;

namespace AgendaApi.Domain.Interfaces
{
    public interface IContactRepository
    {
        Task<Guid> AddAsync(Contact contact);
        Task<Contact?> GetByIdAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(Contact contact, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task<List<Contact>> GetAllAsync(CancellationToken ct = default);
    }
}
