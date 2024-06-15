using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_10.models;

[Table("Medicaments")]
public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(100)]
    public string Description { get; set; }
    [MaxLength(100)]
    public string Type { get; set; }
    
    public ICollection<Prescription_Medicament> PrescriptionMedicaments { get; set; } = new HashSet<Prescription_Medicament>();
}