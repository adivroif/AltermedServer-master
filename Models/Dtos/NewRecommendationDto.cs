using AltermedManager.Models.Entities;

namespace AltermedManager.Models.Dtos
    {
    public class NewRecommendationDto
        {

        public int recommendationId { get; set; }
        public Guid patientId { get; set; }
        public Guid? appointmentId { get; set; }
        public int recommendedTreatmentId { get; set; }
        public DateTime recommendationDate { get; set; } = DateTime.UtcNow;
        public string reason { get; set; }
        public string source { get; set; }
        public bool isChosen { get; set; }
        public Patient Patient { get; set; }
        public Appointment Appointment { get; set; }
        public Treatment RecommendedTreatment { get; set; }

        }
    }
