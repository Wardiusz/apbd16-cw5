namespace Tutorial5.Entities
{
    public class Medicament
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    }
}