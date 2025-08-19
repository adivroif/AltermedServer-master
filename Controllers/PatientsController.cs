using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DummyHealthProviderService _healthProvider;
        private readonly ILogger<AddressController> _log;
        public PatientsController(ApplicationDbContext dbContext, DummyHealthProviderService hp, ILogger<AddressController> log)
            {
            this.dbContext = dbContext;
            this._healthProvider = hp;
            _log = log;

            }
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            var allPatients = dbContext.Patients.ToList();
            return Ok(allPatients);
        }

        [HttpGet("{name}")]
        public IActionResult GetPatientByName(string name)
        {
            var patient = dbContext.Patients
                .Include(p => p.patientAddress) // Load the related patientAddress
                .FirstOrDefault(p => p.patientName + " " + p.patientSurname == name);

            if (patient == null)
            {
                _log.LogWarning($"No patient found with name: {name}");
                return NotFound();
            }

            return Ok(patient);
        }


        [HttpGet("{id:guid}")]
        public Patient? GetPatientByUId(Guid id)
        {
            var patient = dbContext.Patients.Find(id);
            if (patient is null)
            {
                _log.LogWarning($"No patient found with ID: {id}");
                return null;
            }
            return patient;
        }



        [HttpPost]
        public IActionResult AddPatient(NewPatientDto newPatient)
        {
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

            return Ok(patientEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdatePatient(Guid id, UpdatePatientDto updatePatientDto)
        {
            var patient = dbContext.Patients.Find(id);
            if (patient is null)
            {
                _log.LogWarning($"No patient found with ID: {id}");
                return NotFound();
            }
            patient.patientName = updatePatientDto.patientName;
            patient.patientSurname = updatePatientDto.patientSurname;
            patient.patientEmail = updatePatientDto.patientEmail;
            patient.patientPhone = updatePatientDto.patientPhone;
            patient.patientAddress = updatePatientDto.patientAddress;
            patient.healthProvider = updatePatientDto.healthProvider;
            patient.gender = updatePatientDto.gender;

            dbContext.SaveChanges();
            return Ok(patient);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeletePatient(Guid id)
        {
            var patient = dbContext.Patients.Find(id);
            if(patient is null)
            {
                _log.LogWarning($"No patient found with ID: {id}");
                return NotFound();
            }
            dbContext.Patients.Remove(patient);
            dbContext.SaveChanges();
            return Ok("Patient deleted");
        }

        [HttpGet("hp/{idNum}")]
        public async Task<IActionResult> GetFromHealthProvider(string idNum)
            {
            var patient = await _healthProvider.GetPatientByIdAsync(idNum);
            if (patient == null)
                {
                _log.LogWarning($"No patient found with ID number: {idNum} in health provider system.");
                return NotFound();
                }

            return Ok(patient);
            }
        }
}
