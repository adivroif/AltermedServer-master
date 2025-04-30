using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
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
        public PatientsRequestController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

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
                return NotFound("לא נמצאו פניות עבור המטופל.");
            }

            return Ok(requests);
        }


        [HttpGet("{id:guid}")]
        public IActionResult GetPatientRequestByUId(Guid id)
        {
            var patientRequset = dbContext.PatientRequest.Find(id);
            if (patientRequset is null)
            {
                return NotFound();
            }
            return Ok(patientRequset);
        }



        [HttpPost]
        public IActionResult AddPatientRequest(NewPatientRequestDto newPatientRequest)
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
                requestType = newPatientRequest.requestType,
            };
            dbContext.PatientRequest.Add(patientRequestEntity);
            dbContext.SaveChanges();

            return Ok(patientRequestEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdatePatientRequest(Guid id, UpdatePatientRequestDto updatePatientRequestsDto)
        {
            var patientRequest = dbContext.PatientRequest.Find(id);
            if (patientRequest is null)
            {
                return NotFound();
            }
            patientRequest.requestId = updatePatientRequestsDto.requestId;
            patientRequest.appointmentId = updatePatientRequestsDto.appointmentId;
            patientRequest.patientId = updatePatientRequestsDto.patientId;
            patientRequest.createdOn = updatePatientRequestsDto.createdOn;
            patientRequest.description = updatePatientRequestsDto.description;
            patientRequest.doctorId = updatePatientRequestsDto.doctorId;
            patientRequest.isUrgent = updatePatientRequestsDto.isUrgent;
            patientRequest.requestType = updatePatientRequestsDto.requestType;


            dbContext.SaveChanges();
            return Ok(patientRequest);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeletePatientRequest(Guid id)
        {
            var patientRequest = dbContext.PatientRequest.Find(id);
            if(patientRequest is null)
            {
                return NotFound();
            }
            dbContext.PatientRequest.Remove(patientRequest);
            dbContext.SaveChanges();
            return Ok("Patient request deleted");
        }
    }
}
