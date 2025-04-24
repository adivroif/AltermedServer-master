using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;

namespace AltermedManager.Models.Dtos
    {
    public class NewAppointmentDto
        {
        public Guid appointmentId { get; set; }
        public int startSlot { get; set; }
        public int treatmentId { get; set; }
        public Guid patientId { get; set; }
        public Guid doctorId { get; set; }
        public Guid recordId { get; set; }
        public Address Address { get; set; }
        public int duration { get; set; }
        public Status statusOfAppointment { get; set; }
        }
    }
