using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
    {
    public class Recommendation
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int recommendationId { get; set; }
        public Guid patientId { get; set; }
        public Guid? appointmentId { get; set; }
        public int recommendedTreatmentId { get; set; }
        public DateTime recommendationDate { get; set; } = DateTime.UtcNow;
        public string? reason { get; set; }
        public string? source { get; set; }
        public bool isChosen { get; set; }
        public required Patient Patient { get; set; }
        public Appointment? Appointment { get; set; }
        public required Treatment RecommendedTreatment { get; set; }

        }
    }