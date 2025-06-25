using AltermedManager.Models.Entities;

namespace AltermedManager.Models.Dtos
{
    public class NewPatientDto
    {
        public Guid id { get; set; }
        public required string patientID { get; set; }
        public required string patientName { get; set; }
        public required string patientSurname { get; set; }
        public required string patientEmail { get; set; }
        public string? patientPhone { get; set; }
        public required Address patientAddress { get; set; }
        public required string healthProvider { get; set; }
        public char gender { get; set; }
        public DateTime dateOfBirth { get; set; }
    }
}
