using Microsoft.EntityFrameworkCore;
using guialocal.Models;

namespace guialocal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
        public DbSet<Customer> Customers {  get; set; }
    }
}