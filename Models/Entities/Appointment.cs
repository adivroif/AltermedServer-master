using AltermedManager.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid appointmentId { get; set; }
        public int startSlot { get; set; }
        [ForeignKey("startSlot")]
        public AppointmentSlots startAppSlot { get; set; }
        public int treatmentId { get; set; }
        public Guid patientId { get; set; }
        public Guid doctorId { get; set; }
        public Guid recordId { get; set; }

        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address Address { get; set; }
        public int duration { get; set; }
        public Status statusOfAppointment { get; set; }
        
    }
}
