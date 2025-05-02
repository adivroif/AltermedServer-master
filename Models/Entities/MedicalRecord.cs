using AltermedManager.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
{
    public class MedicalRecord
    {
        [Key]
        public Guid recordID { get; set; }
        public Guid appointmentId { get; set; }
        public Guid patientId { get; set; }
        public required string description { get; set; }
        public Guid createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public MedicalRecordType recordType { get; set; }
    }
}
