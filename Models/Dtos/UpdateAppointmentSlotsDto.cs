using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;

namespace AltermedManager.Models.Dtos
    {
    public class UpdateAppointmentSlotsDto
        {
        public int slotid { get; set; }
        public Guid doctorid { get; set; }
        public string date_of_treatment { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public int isbooked { get; set; }
    }
    }
