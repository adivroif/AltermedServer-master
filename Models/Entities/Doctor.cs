
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
    {
    public class Doctor
        {
        [Key]
        
        public Guid DoctorId { get; set; }
        public required string doctorLicense { get; set; }
        public required string doctorName { get; set; }
        public required string doctorSurname { get; set; }
        public List<string> specList { get; set; }
        public Guid scheduleId { get; set; }
        public List<int> placesWorking {  get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }


    }
}
