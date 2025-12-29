namespace VetClinicSystem.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public string MicrochipId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }

        public int VetDoctorId { get; set; }
        public VetDoctor VetDoctor { get; set; }

        public PetProfile PetProfile { get; set; }
    }
}
