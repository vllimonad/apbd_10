using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbd_10.models;

[Table("Prescriptions_Medicaments")]
[PrimaryKey(nameof(IdPrescription), nameof(IdMedicament))]
public class Prescription_Medicament
{
    public int IdMedicament { get; set; }
    public int IdPrescription { get; set; }
    public int Dose { get; set; }
    public string Details { get; set; }

    [ForeignKey(nameof(IdMedicament))]
    [Required]
    public Medicament Medicament { get; set; } = null!;
    
    [ForeignKey(nameof(IdPrescription))]
    public Prescription Prescription { get; set; } = null!;
}