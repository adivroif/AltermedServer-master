using AltermedManager.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
{
    public class PatientRequest
    {
        [Key]
        public Guid requestId { get; set; }
        public Guid patientId { get; set; }
        public Guid doctorId { get; set; }
        public required string description { get; set; }
        public DateTime createdOn { get; set; }
        public RequestType requestType { get; set; }
        public Guid appointmentId { get; set; }
        public string? answerFromDoctor { get; set; }

        public bool isUrgent { get; set; }
        
    }
}
