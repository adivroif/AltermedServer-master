using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;

namespace AltermedManager.Models.Dtos
    {
    public class UpdatePatientRequestDto
        {
        public Guid requestId { get; set; }
        public Guid patientId { get; set; }
        public Guid doctorId { get; set; }
        public string description { get; set; }
        public DateTime createdOn { get; set; }
        public RequestType requestType { get; set; }
        public int treatmentId { get; set; }
        public bool isUrgent { get; set; }
    }
    }
