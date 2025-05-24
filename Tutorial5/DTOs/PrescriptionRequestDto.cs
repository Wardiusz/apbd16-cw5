namespace Tutorial5.DTOs
{
    public class PrescriptionRequestDto
    {
        public PatientDto Patient { get; set; }
        public DoctorDto Doctor { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public List<MedicamentDto> Medicaments { get; set; }
    }
}