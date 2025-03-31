using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System;

namespace AltermedManager.Models.Entities
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string patientID { get; set; }
        public string patientName { get; set; }
        public string patientSurname { get; set; }
        public string patientEmail { get; set; }
        public string patientPhone { get; set; }
        public Address patientAddress { get; set; }
        public string healthProvider { get; set; }
        public char gender { get; set; }
        public DateTime dateOfBirth { get; set; }
        public List<Appointment> appointments { get; set; }
        public List<MedicalRecord> medicalRecords { get; set; }
        public List<PatientRequest> requests { get; set; }

    }
}


