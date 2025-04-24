using AltermedManager.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Dtos
{
    public class NewDoctorScheduleDto
    {
        public Guid scheduleid { get; set; }
        public string doctorid { get; set; }
        public string date { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string slotid { get; set; }
        public Address address { get; set; }

    }
}
