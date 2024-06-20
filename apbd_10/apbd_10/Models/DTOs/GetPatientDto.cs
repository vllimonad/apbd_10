using apbd_10.models;

namespace apbd_10.Models.DTOs;

public class GetPatientDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public ICollection<GetPrescriptionDto> prescriptions { get; set; }
    
}

public class GetPrescriptionDto
{
    public int IdPrescription { get; set; }
    public DateTime date { get; set; }
    public DateTime dueDate { get; set; }
    public ICollection<MedicamentDto> medicaments { get; set; }
    public DoctorDto doctor { get; set; }
}