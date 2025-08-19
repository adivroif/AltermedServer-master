using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace AltermedManager.Services
    {
    public class PatientService
        {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<PatientService> _log;

        public PatientService(ApplicationDbContext dbContext, ILogger<PatientService> log)
            {
            this.dbContext = dbContext;
            _log = log;
            }


        public List<Patient> GetAllPatients()
            {
            return dbContext.Patients.ToList();
            }

        public Patient? GetPatientByName(string name)
            {
            if (string.IsNullOrWhiteSpace(name))
                {
                _log.LogWarning("Patient name is null or empty.");
                return null;

                }
            var patient = dbContext.Patients
                                            .Include(p => p.patientAddress) // Load the related patientAddress
                                            .FirstOrDefault(p => p.patientName + " " + p.patientSurname == name);
            if (patient == null)
                {
                _log.LogWarning($"No patient found with name: {name}");
                return null;
                }
            return patient;
            }

        public Patient AddPatient(NewPatientDto newPatient)
            {
            if (newPatient == null)
                {
                _log.LogWarning("New patient data is null.");
                return null;
                }
            var patientEntity = new Patient()
                {
                id = newPatient.id,
                patientID = newPatient.patientID,
                patientName = newPatient.patientName,
                patientSurname = newPatient.patientSurname,
                patientEmail = newPatient.patientEmail,
                patientPhone = newPatient.patientPhone,
                patientAddress = newPatient.patientAddress,
                healthProvider = newPatient.healthProvider,
                gender = newPatient.gender,
                dateOfBirth = newPatient.dateOfBirth
                };
            dbContext.Patients.Add(patientEntity);
            dbContext.SaveChanges();
            _log.LogInformation($"New patient added with ID: {patientEntity.id}");
            return patientEntity;
            }
        public Patient? UpdatePatient(Guid id, UpdatePatientDto updatePatientDto)
            {

            var patient = dbContext.Patients.Find(id);
            if (patient is null)
                {
                _log.LogWarning($"No patient found with ID: {id}");
                return null;
                }
            patient.patientName = updatePatientDto.patientName;
            patient.patientSurname = updatePatientDto.patientSurname;
            patient.patientEmail = updatePatientDto.patientEmail;
            patient.patientPhone = updatePatientDto.patientPhone;
            patient.patientAddress = updatePatientDto.patientAddress;
            patient.healthProvider = updatePatientDto.healthProvider;
            patient.gender = updatePatientDto.gender;

            dbContext.SaveChanges();
            return patient;

            }
        public bool DeletePatient(Guid id)
            {
            var patient = dbContext.Patients.Find(id);
            if (patient is null)
                {
                _log.LogWarning($"No patient found with ID: {id}");
                return false;
                }
            dbContext.Patients.Remove(patient);
            dbContext.SaveChanges();
            return true;
            }


        }
    }
