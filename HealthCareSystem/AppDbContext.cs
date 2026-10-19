using Microsoft.EntityFrameworkCore;

namespace HealthCareSystem;

public class AppDbContext : DbContext
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // For SQL Server use: options.UseSqlServer("Server=.;Database=HealthCareDb;Trusted_Connection=True;TrustServerCertificate=True;");
        options.UseSqlite("Data Source=healthcare.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Many-to-Many: Patient <-> Doctor through Appointment.
        // Composite key includes the date so the same patient can see the same doctor on different days.
        modelBuilder.Entity<Appointment>()
            .HasKey(a => new { a.PatientId, a.DoctorId, a.AppointmentDate });

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId);
    }
}
