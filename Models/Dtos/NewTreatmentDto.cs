using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;

namespace AltermedManager.Models.Dtos
    {
    public class NewTreatmentDto
        {
        public int treatmentId { get; set; }
        public string treatmentName { get; set; }
        public string treatmentDescription { get; set; }
        public decimal treatmentPrice { get; set; }
        public int treatmentDuration { get; set; }
        public List<string> suitCategories { get; set; }
        public string treatmentGroup { get; set; }
        public bool isAdvanced { get; set; } //if treatment is invasive and more aggressive or not
        public int numOfFeedbacks { get; set; }
        public float score { get; set; }

        }
    }
