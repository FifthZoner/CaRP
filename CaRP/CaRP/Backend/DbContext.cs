using Microsoft.EntityFrameworkCore;

namespace CaRP.Backend;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions<DbContext> options) : base(options)
    {
    }

    public DbSet<Session> UserProfiles { get; set; }
}

public class Session
{
    public long Id { get; set; }
    public string Login { get; set; } = string.Empty;
}