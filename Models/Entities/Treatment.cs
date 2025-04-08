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
        public string treatmentName { get; set; }
        public string treatmentDescription { get; set; }
        public decimal treatmentPrice { get; set; }
        //public Address treatmentPlace { get; set; }
        //public Address treatmentPlace { get; set; }
        public List<SuitCategories> suitCategories { get; set; }

        }
    }
    