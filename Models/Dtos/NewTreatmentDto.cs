using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;

namespace AltermedManager.Models.Dtos
    {
    public class NewTreatmentDto
        {
        public int treatmentId { get; set; }
        public string treatmetName { get; set; }
        public string treatmentDescription { get; set; }
        public decimal treatmentPrice { get; set; }
        //public Address treatmentPlace { get; set; }
        //public Address place { get; set; }
        public List<suitCategories> suitCategories { get; set; }
        }
    }
