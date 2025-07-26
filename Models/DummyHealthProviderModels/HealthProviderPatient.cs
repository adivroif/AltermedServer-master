using Microsoft.AspNetCore.Mvc;

namespace AltermedManager.Models.DummyHealthProviderModels
{
    public class HealthProviderPatient 
    {
        
        public required string patientID { get; set; }
        public required string patientName { get; set; }
        public required string patientSurname { get; set; }
        public string? patientEmail { get; set; }
        public string? patientPhone { get; set; }
        public Address patientAddress { get; set; }
        public required string healthProvider { get; set; }
        public char gender { get; set; }
        public DateTime dateOfBirth { get; set; }
        }
}
