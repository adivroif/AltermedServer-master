using AltermedManager.Models.Entities;

namespace AltermedManager.Models.Dtos
{
    public class NewPatientDto
    {

        public string patientID { get; set; }
        public string patientName { get; set; }
        public string patientSurname { get; set; }
        public string patientEmail { get; set; }
        public string patientPhone { get; set; }
        public Address patientAddress { get; set; }
        public string healthProvider { get; set; }
        public char gender { get; set; }
        public DateTime dateOfBirth { get; set; }
    }
}
