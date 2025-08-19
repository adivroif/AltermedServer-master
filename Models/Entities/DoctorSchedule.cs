
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
    {
    public class DoctorSchedule
        {
        [Key]
        
        public Guid scheduleid { get; set; }
        public string doctorid { get; set; }
        public string date { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public int[] slotsid { get; set; }
        public int addressId { get; set; }

        public Address Address { get; set; }
        
        }
    }
