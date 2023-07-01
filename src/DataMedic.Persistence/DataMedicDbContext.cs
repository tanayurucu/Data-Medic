using Microsoft.EntityFrameworkCore;

namespace DataMedic.Persistence;

public class DataMedicDbContext : DbContext
{
    public DataMedicDbContext(DbContextOptions<DataMedicDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(PersistenceAssembly.Assembly);
    }
}