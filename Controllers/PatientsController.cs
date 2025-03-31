using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
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
        public PatientsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            var allPatients = dbContext.Patients.ToList();
            return Ok(allPatients);
        }



        [HttpGet("pasport{id}")]
        public async Task<IActionResult> GetPatientById(string id)
        {
            var patient = await dbContext.Patients
                .Where(p => p.patientID == id)
                .FirstOrDefaultAsync();
            if (patient is null)
            {
                return NotFound();
            }
            return Ok(patient);
        }


        [HttpGet("{id:guid}")]
        public IActionResult GetPatientByUId(Guid id)
        {
            var patient = dbContext.Patients.Find(id);
            if (patient is null)
            {
                return NotFound();
            }
            return Ok(patient);
        }



        [HttpPost]
        public IActionResult AddPatient(NewPatientDto newPatient)
        {
            var patientEntity = new Patient()
            {
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
                return NotFound();
            }
            dbContext.Patients.Remove(patient);
            dbContext.SaveChanges();
            return Ok("Patient deleted");
        }
    }
}
