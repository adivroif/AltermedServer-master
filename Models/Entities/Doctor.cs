using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
    {
    public class Doctor
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DoctorId { get; set; }
        public string doctorLicense { get; set; }
        public string doctorName { get; set; }
        public string doctorSurname { get; set; }
        public List<string> specList { get; set; }
        
        }
    }
