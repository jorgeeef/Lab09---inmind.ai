using Microsoft.EntityFrameworkCore;

namespace ChatService1;

public class ChtDbContext : DbContext
{
    public ChtDbContext(DbContextOptions<ChtDbContext> options) : base(options)
    {
    }
    public DbSet<ChatMessage> ChatMessages { get; set; }
}