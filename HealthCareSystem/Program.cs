using HealthCareSystem;
using Microsoft.EntityFrameworkCore;

using var db = new AppDbContext();
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

// ---------- Seed data ----------
var cardio = new Doctor { Name = "Dr. Khaled Mansour", Specialization = "Cardiology" };
var derma = new Doctor { Name = "Dr. Nadia Fawzy", Specialization = "Dermatology" };

var hany = new Patient { Name = "Hany Samir", DateOfBirth = new DateTime(1990, 3, 14) };
var laila = new Patient { Name = "Laila Adel", DateOfBirth = new DateTime(1985, 11, 2) };

db.Appointments.AddRange(
    new Appointment { Patient = hany, Doctor = cardio, AppointmentDate = new DateTime(2026, 10, 22, 10, 0, 0) },
    new Appointment { Patient = hany, Doctor = derma, AppointmentDate = new DateTime(2026, 10, 25, 12, 30, 0) },
    new Appointment { Patient = laila, Doctor = cardio, AppointmentDate = new DateTime(2026, 10, 23, 9, 0, 0) },
    new Appointment { Patient = hany, Doctor = cardio, AppointmentDate = new DateTime(2026, 11, 5, 10, 0, 0) }
);
db.SaveChanges();

// ---------- Queries using navigation properties ----------
Console.WriteLine("=== Patients and their Doctors (Many-to-Many through Appointment) ===");
var patients = db.Patients
    .Include(p => p.Appointments)
        .ThenInclude(a => a.Doctor)
    .ToList();

foreach (var p in patients)
{
    Console.WriteLine($"{p.Name} (born {p.DateOfBirth:yyyy-MM-dd})");
    foreach (var a in p.Appointments.OrderBy(a => a.AppointmentDate))
        Console.WriteLine($"   {a.AppointmentDate:yyyy-MM-dd HH:mm} with {a.Doctor.Name} ({a.Doctor.Specialization})");
}

Console.WriteLine("\n=== Doctors and their Patients (Doctor -> Patients) ===");
var doctors = db.Doctors
    .Include(d => d.Appointments)
        .ThenInclude(a => a.Patient)
    .ToList();

foreach (var d in doctors)
{
    Console.WriteLine($"{d.Name} - {d.Specialization}");
    foreach (var a in d.Appointments.OrderBy(a => a.AppointmentDate))
        Console.WriteLine($"   {a.AppointmentDate:yyyy-MM-dd HH:mm} - {a.Patient.Name}");
}

Console.WriteLine("\n=== Distinct patients per doctor ===");
foreach (var d in doctors)
{
    var count = d.Appointments.Select(a => a.PatientId).Distinct().Count();
    Console.WriteLine($"{d.Name}: {count} patient(s)");
}
