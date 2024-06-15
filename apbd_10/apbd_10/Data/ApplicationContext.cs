using apbd_10.models;
using Microsoft.EntityFrameworkCore;

namespace apbd_10.Data;

public class ApplicationContext: DbContext
{
    protected ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Prescription_Medicament> PrescriptionMedicaments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>()
        {
            new() { IdDoctor = 1, FirstName = "John", LastName = "Doe", Email = "wecf"},
            new() { IdDoctor = 2, FirstName = "Ann", LastName = "Smith", Email = "wecf"},
            new() { IdDoctor = 3, FirstName = "Jack", LastName = "Taylor", Email = "wecf"}
        });
        
        modelBuilder.Entity<Patient>().HasData(new List<Patient>()
        {
            new() { IdPatient = 1, FirstName = "John", LastName = "Doe", BirthDate = DateTime.Today},
            new() { IdPatient = 2, FirstName = "Ann", LastName = "Smith", BirthDate = DateTime.Today},
            new() { IdPatient = 3, FirstName = "Jack", LastName = "Taylor", BirthDate = DateTime.Today}
        });
        
        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>()
        {
            new() { IdPrescription = 1, IdDoctor = 1, IdPatient = 1, Date = DateTime.Today, DueDate = DateTime.Today},
            new() { IdPrescription = 2, IdDoctor = 1, IdPatient = 2, Date = DateTime.Today, DueDate = DateTime.Today},
            new() { IdPrescription = 3, IdDoctor = 2, IdPatient = 2, Date = DateTime.Today, DueDate = DateTime.Today}
        });
        
        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>()
        {
            new() { IdMedicament = 1, Name = "a", Description = "adcdc", Type = "A"},
            new() { IdMedicament = 2, Name = "b", Description = "adcdc", Type = "A"},
            new() { IdMedicament = 3, Name = "c", Description = "adcdc", Type = "A"}
        });
        
        modelBuilder.Entity<Prescription_Medicament>().HasData(new List<Prescription_Medicament>()
        {
            new() { IdMedicament = 1, IdPrescription = 1, Dose = 3, Details = "adcdc"},
            new() { IdMedicament = 2, IdPrescription = 1, Dose = 3, Details = "adcdc"},
            new() { IdMedicament = 3, IdPrescription = 2, Dose = 3, Details = "adcdc"}
        });
    }
}