using AgendaApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgendaApi.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Contact> Contacts => Set<Contact>();
    }
}
