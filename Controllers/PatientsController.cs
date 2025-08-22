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
        private readonly PatientService _patientService;
        public PatientsController(ApplicationDbContext dbContext, DummyHealthProviderService hp, ILogger<AddressController> log, PatientService patientService)
            {
            this.dbContext = dbContext;
            this._healthProvider = hp;
            this._patientService = patientService;
            _log = log;

            }
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            var allPatients = dbContext.Patients.ToList();
            return Ok(allPatients);

            return Ok(_patientService.GetAllPatients());
            }

        [HttpGet("{name}")]
        public IActionResult GetPatientByName(string name)
        {
            var patient = _patientService.GetPatientByName(name);
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
        public IActionResult AddPatient(NewPatientDto newDto)
        {
            var newPatient = _patientService.AddPatient(newDto);
            if (newPatient == null)
                {
                _log.LogError("Failed to add new patient.");
                return BadRequest("Failed to add new patient.");
                }
            return Ok(newPatient);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdatePatient(Guid id, UpdatePatientDto updatePatientDto)
        {
           
            var updated = _patientService.UpdatePatient(id, updatePatientDto);
            if (updated is null)
                {
                _log.LogWarning($"Failed to update patient with ID: {id}");
                return NotFound();
                }
            return Ok(updated);

        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeletePatient(Guid id)
        {
            if(_patientService.DeletePatient(id) is true)
                {
                _log.LogInformation($"Patient with ID: {id} deleted successfully.");
                return Ok("Patient deleted");
                }
            else
                {
                _log.LogWarning($"Failed to delete patient with ID: {id}. Patient not found.");
                return NotFound();
                }
            
        }

        //------------------------------------------------
        // Health Provider Integration  
        //------------------------------------------------

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
