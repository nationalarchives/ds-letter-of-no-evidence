using Microsoft.EntityFrameworkCore;
using letter_of_no_evidence.domain;

namespace letter_of_no_evidence.data
{
    public class LONEDBContext : DbContext
    {
        public LONEDBContext(DbContextOptions<LONEDBContext> options)
            : base(options)
        { }

        public DbSet<Request> Requests { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentStatus>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<PaymentStatus>().Property(e => e.Description).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<PaymentStatus>().Property(e => e.Meaning).IsRequired().HasMaxLength(250);

            modelBuilder.Entity<Payment>().Property(e => e.SessionId).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Payment>().Property(e => e.PaymentId).HasMaxLength(30);
            modelBuilder.Entity<Payment>().Property(e => e.PaymentStatusId).IsRequired();
            modelBuilder.Entity<Payment>().Property(e => e.Amount).IsRequired().HasPrecision(4, 2);

            modelBuilder.Entity<Request>().Property(e => e.RequestNumber).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Request>().Property(e => e.SubjectFirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Request>().Property(e => e.SubjectLastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Request>().Property(e => e.AlternativeFirstName).HasMaxLength(50);
            modelBuilder.Entity<Request>().Property(e => e.AlternativeLastName).HasMaxLength(50);
            modelBuilder.Entity<Request>().Property(e => e.DateOfBirth).HasMaxLength(30);
            modelBuilder.Entity<Request>().Property(e => e.DateOfDeath).HasMaxLength(30);
            modelBuilder.Entity<Request>().Property(e => e.CountryOfBirth).HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.ContactTitle).HasMaxLength(30);
            modelBuilder.Entity<Request>().Property(e => e.ContactFirstName).HasMaxLength(50);
            modelBuilder.Entity<Request>().Property(e => e.ContactLastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Request>().Property(e => e.ContactEmail).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.ContactAddress1).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.ContactAddress2).HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.ContactCity).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.ContactCounty).HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.ContactPostCode).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Request>().Property(e => e.ContactCountry).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.AgentFullName).HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.AgentAddress1).HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.AgentAddress2).HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.AgentCity).HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.AgentCounty).HasMaxLength(100);
            modelBuilder.Entity<Request>().Property(e => e.AgentPostCode).HasMaxLength(30);
            modelBuilder.Entity<Request>().Property(e => e.AgentCountry).HasMaxLength(100);
        }
    }
}
