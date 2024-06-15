using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbd_10.models;

[Table("Prescriptions")]
[PrimaryKey(nameof(IdPrescription))]
public class Prescription
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }

    [ForeignKey(nameof(IdPatient))]
    public Patient Patient { get; set; } = null!;
    
    [ForeignKey(nameof(IdDoctor))]
    public Doctor Doctor { get; set; } = null!;
    
    public ICollection<Prescription_Medicament> PrescriptionMedicaments { get; set; } = new HashSet<Prescription_Medicament>();
}