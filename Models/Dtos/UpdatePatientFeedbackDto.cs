using AltermedManager.Models.Entities;

namespace AltermedManager.Models.Dtos
{
    public class UpdatePatientFeedbackDto
    {
        public Guid feedbackId { get; set; }
        public Guid patientId { get; set; }
        public Guid appointmentId { get; set; }
        public int overallStatus { get; set; }
        public string newSymptoms { get; set; }
        public string comments { get; set; }
        public DateTime createdOn { get; set; }

    }
}
