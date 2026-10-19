namespace HealthCareSystem;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }

    // Many-to-Many with Doctor (through Appointment)
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;

    // Many-to-Many with Patient (through Appointment)
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

// Join entity: Patient <-> Doctor
public class Appointment
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }

    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
}
