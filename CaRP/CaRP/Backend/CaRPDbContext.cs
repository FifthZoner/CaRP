using CaRP.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CaRP.Backend;

public class CaRpDbContext(DbContextOptions<CaRpDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Servicing> Services { get; set; }
    public DbSet<WorkRegistration> WorkRegistrations { get; set; }
}

public class Session
{
    public long Id { get; set; }
    public string Login { get; set; } = string.Empty;
}