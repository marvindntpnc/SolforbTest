using Microsoft.EntityFrameworkCore;
using SolforbTest.Domain;

namespace SolforbTest.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<ReceiptDocument> ReceiptDocuments { get; set; }
    }
}
