namespace VetClinicSystem.Models
{
    public class PetProfile
    {
        public int Id { get; set; }

        public int PetId { get; set; }
        public Pet Pet { get; set; }

        public string VetNotes { get; set; }
    }
}
