using AltermedManager.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Dtos
{
    public class UpdateDoctorDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DoctorId { get; set; }
        public required string doctorLicense { get; set; }
        public required string doctorName { get; set; }
        public required string doctorSurname { get; set; }
        public List<string>? specList { get; set; }
        public Guid scheduleId { get; set; }
        public List<int> placesWorking { get; set; } = new List<int>();

        }
}
