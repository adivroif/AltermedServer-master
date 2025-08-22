using AltermedManager.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AltermedManager.Models.Entities
    {
    public class Treatment
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int treatmentId { get; set; }
        public required string treatmentName { get; set; }
        public string? treatmentDescription { get; set; }
        public decimal treatmentPrice { get; set; }
        public int treatmentDuration {  get; set; }
        public List<string>? suitCategories { get; set; }
        public required string treatmentGroup { get; set; }
        public bool isAdvanced { get; set; }  //if treatment is invasive and more aggressive or not
        public int numOfFeedbacks { get; set; }
        public float score { get; set; }
        public int treatmentPlaceId { get; set; }


    }
}
    