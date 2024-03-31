using Microsoft.EntityFrameworkCore;
using Schemes.Models;

namespace Schemes.Repository
{
    public class SchemesContext :DbContext
    {
        public SchemesContext()
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerLogin> CustomerLogin { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<AdminLogin> AdminLogin { get; set; }
        public DbSet<SchemesDetails> SchemesDetails { get; set; }
        public DbSet<OTPDetails> OTPDetails { get; set; }
        public DbSet<LoanInterestDetails> LoanInterestDetails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=TEJU;Initial Catalog=SchemesDatabase;Integrated Security=True;TrustServerCertificate=True");
            }
        }
    }

}
