namespace Tutorial5.Entities
{
    public class Doctor
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}