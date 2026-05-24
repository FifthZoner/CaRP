using CaRP.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CaRP.Backend;

internal class CaRpDbContext(DbContextOptions<CaRpDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Servicing> Servicing { get; set; }
    public DbSet<WorkRegistration> WorkRegistrations { get; set; }
}
