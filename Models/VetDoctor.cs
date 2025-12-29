using System.Collections.Generic;

namespace VetClinicSystem.Models
{
    public class VetDoctor
    {
        public int Id { get; set; }          // PK
        public string Name { get; set; }
        public string Specialty { get; set; }

        // Navigation: one doctor has many pets
        public ICollection<Pet> Pets { get; set; }
    }
}
