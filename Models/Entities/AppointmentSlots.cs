using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
{
    public class AppointmentSlots
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int slotid { get; set; }
        public Guid doctorid { get; set; }
        public DateOnly date_of_treatment { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public int isbooked { get; set; }
    }
}
