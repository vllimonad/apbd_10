using System.ComponentModel.DataAnnotations;

namespace apbd_10.models.DTOs;

public class NewPrescriptionDto
{
    [Required]
    public PatientDto patient { get; set; }
    [Required]
    public DoctorDto doctor { get; set; }
    [Required]
    public List<MedicamentDto> medicaments { get; set; } = new List<MedicamentDto>();
    [Required]
    public DateTime date { get; set; }
    [Required]
    public DateTime dueDate { get; set; }
}