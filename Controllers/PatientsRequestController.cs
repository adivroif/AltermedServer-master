using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsRequestController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<AddressController> _log;
        public PatientsRequestController(ApplicationDbContext dbContext, INotificationsService notificationsService, ILogger<AddressController> log)
            {
            this.dbContext = dbContext;
            _notificationsService = notificationsService;
            _log = log;
            }
        [HttpGet]
        public IActionResult GetAllPatientsRequests()
        {
            var allPatientsRequestes = dbContext.PatientRequest.ToList();
            return Ok(allPatientsRequestes);
        }


        [HttpGet("patient/{patientId}")]
        public IActionResult GetRequestsByPatientId(string patientId)
        {
            var requests = dbContext.PatientRequest
                .Where(r => r.patientId.ToString() == patientId)
                .ToList();

            if (requests == null || requests.Count == 0)
            {
                _log.LogWarning($"No requests found for patient with ID: {patientId}");
                return NotFound("Requests not found.");
                }

            return Ok(requests);
        }


        [HttpGet("doctor/{doctorId}")]
        public IActionResult GetRequestsByDoctorId(string doctorId)
        {
            var requests = dbContext.PatientRequest
                .Where(r => r.doctorId.ToString() == doctorId)
                .ToList();

            if (requests == null || requests.Count == 0)
            {
                _log.LogWarning($"No requests found for doctor with ID: {doctorId}");
                return NotFound("Requests not found.");
            }

            return Ok(requests);
        }


        [HttpGet("{id:guid}")]
        public IActionResult GetPatientRequestByUId(Guid id)
        {
            var patientRequset = dbContext.PatientRequest.Find(id);
            if (patientRequset is null)
            {
                _log.LogWarning($"No patient request found with ID: {id}");
                return NotFound();
            }
            return Ok(patientRequset);
        }



        [HttpPost]
        public async Task<IActionResult> AddPatientRequestAsync(NewPatientRequestDto newPatientRequest)
        {
            Console.WriteLine(newPatientRequest.patientId);

            var patientRequestEntity = new PatientRequest()
            {
                requestId = newPatientRequest.requestId,
                appointmentId = newPatientRequest.appointmentId,
                patientId = newPatientRequest.patientId,
                createdOn = newPatientRequest.createdOn,
                description = newPatientRequest.description,
                doctorId = newPatientRequest.doctorId,
                isUrgent = newPatientRequest.isUrgent,
                answerFromDoctor = newPatientRequest.answerFromDoctor,
                requestType = newPatientRequest.requestType,
            };
            dbContext.PatientRequest.Add(patientRequestEntity);
            dbContext.SaveChanges();
            dbContext.Entry(patientRequestEntity).Reload();

            // Send notification to doctor about new request from patient
            Guid doctorId = patientRequestEntity.doctorId;
            string? msgToken = dbContext.Users
                .Where(u => u.id == doctorId)
                .Select(u => u.msgToken)
                .FirstOrDefault();
            Guid newRequestId = patientRequestEntity.requestId;
            await _notificationsService.SendNewPatientRequestToDoctor(doctorId, patientRequestEntity, msgToken);
            _log.LogInformation($"New request created and sent to doctor with ID: {doctorId}");
            return Ok(patientRequestEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePatientRequestAsync(Guid id, UpdatePatientRequestDto updatePatientRequestsDto)
        {
            var patientRequest = dbContext.PatientRequest.Find(id);
            if (patientRequest is null)
            {
                _log.LogWarning($"No patient request found with ID: {id}");
                return NotFound();
            }
            patientRequest.requestId = updatePatientRequestsDto.requestId;
            patientRequest.appointmentId = updatePatientRequestsDto.appointmentId;
            patientRequest.patientId = updatePatientRequestsDto.patientId;
            patientRequest.createdOn = updatePatientRequestsDto.createdOn;
            patientRequest.description = updatePatientRequestsDto.description;
            patientRequest.doctorId = updatePatientRequestsDto.doctorId;
            patientRequest.isUrgent = updatePatientRequestsDto.isUrgent;
            patientRequest.answerFromDoctor = updatePatientRequestsDto.answerFromDoctor;
            patientRequest.requestType = updatePatientRequestsDto.requestType;
            dbContext.SaveChanges();
            
            // Send notification to patient about new response from doctor
            Guid patientId = patientRequest.patientId;
            string? msgToken = dbContext.Users
                .Where(u => u.id == patientId)
                .Select(u => u.msgToken)
                .FirstOrDefault();
            await _notificationsService.SendNewDoctorResponseToPatient(patientId, patientRequest, msgToken);
            _log.LogInformation($"Patient request with ID: {id} updated and notification sent to patient with ID: {patientId}");
            return Ok(patientRequest);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeletePatientRequest(Guid id)
        {
            var patientRequest = dbContext.PatientRequest.Find(id);
            if(patientRequest is null)
            {
                _log.LogWarning($"No patient request found with ID: {id}");
                return NotFound();
            }
            dbContext.PatientRequest.Remove(patientRequest);
            dbContext.SaveChanges();
            return Ok("Patient request deleted");
        }
    }
}
