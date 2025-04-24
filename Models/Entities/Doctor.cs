using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
    {
    public class Doctor
        {
        [Key]
        
        public Guid DoctorId { get; set; }
        public string doctorLicense { get; set; }
        public string doctorName { get; set; }
        public string doctorSurname { get; set; }
        public List<string> specList { get; set; }
        public string scheduleId { get; set; }
        public List<string> placesWorking {  get; set; }
        
        }
    }
