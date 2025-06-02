using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;

namespace AltermedManager.Models.Dtos
{
    public class UpdateRecommendationDto
    {
        public int recommendationId { get; set; }
        public Guid patientId { get; set; }
        public Guid? appointmentId { get; set; }
        public int recommendedTreatmentId { get; set; }
        public DateTime recommendationDate { get; set; } = DateTime.UtcNow;
        public string? reason { get; set; }
        public string? source { get; set; }
        public bool? isChosen { get; set; }
        public required Treatment RecommendedTreatment { get; set; }
    }
}
