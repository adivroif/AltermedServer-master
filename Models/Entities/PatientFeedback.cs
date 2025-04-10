using AltermedManager.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
    {
    public class PatientFeedback
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid feedbackId { get; set; }
        public Guid patientId { get; set; }
        public Guid appointmentId { get; set; }
        public int overallStatus { get; set; }
        public string newSymptoms { get; set; }               
        public string comments { get; set; }
        public DateTime createdOn { get; set; }
        }
    }
