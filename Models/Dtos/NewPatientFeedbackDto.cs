namespace AltermedManager.Models.Dtos
    {
    public class NewPatientFeedbackDto
        {
             public Guid feedbackId { get; set; }
             public Guid patientId { get; set; }
             public Guid appointmentId { get; set; }
             public float overallStatus { get; set; }
             public string newSymptoms { get; set; }
             public string comments { get; set; }
             public DateTime createdOn { get; set; }
             public string bodyPart { get; set; }
        }
    }
