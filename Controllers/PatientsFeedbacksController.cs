using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using AltermedManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace AltermedManager.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsFeedbacksController : ControllerBase
        {
        private readonly PatientsFeedbacksService _feedbacksService;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<AddressController> _log;


        public PatientsFeedbacksController(PatientsFeedbacksService feedbacksService, ILogger<AddressController> log)
            {
            _feedbacksService = feedbacksService;
            _log = log;
            }

        [HttpGet]
        public IActionResult GetAllPatientsFeedbacks()
            {
            var result = _feedbacksService.GetAllPatientsFeedbacks();
            if (result == null)
                {
                _log.LogWarning("No feedbacks found.");
                return NotFound("No feedbacks found.");
                }

            return Ok(result);
            }



        [HttpGet("patientId{id}")]
        public async Task<IActionResult> GetPatientFeedbackById(Guid id)
            {
            var result = await _feedbacksService.GetFeedbacksByPatientId(id);
            if (result == null)
                {
                _log.LogWarning($"No feedbacks found for patient with ID: {id}");
                return NotFound("No feedbacks found.");
                }
            return Ok(result);
            }


        [HttpGet("{id:guid}")]
        public IActionResult GetPatientFeedbackByUId(Guid id)
            {
            var feedback = _feedbacksService.GetFeedbackByUId(id);
            if (feedback is null)
                {
                _log.LogWarning($"No feedback found with ID: {id}");
                return NotFound();
                }
            return Ok(feedback);
            }

        [HttpGet("appointmentId/{appointmentId}")]
        public async Task<IActionResult> GetPatientFeedbackByAppointmentId(Guid appointmentId)
        {
            var feedback = await _feedbacksService.GetFeedbackByAppointmentId(appointmentId);

            if (feedback is null)
            {
                _log.LogWarning($"No feedback found for appointment with ID: {appointmentId}");
                return NotFound();
            }
            return Ok(feedback);
        }

        [HttpPost]
        public IActionResult AddPatientFeedback(NewPatientFeedbackDto newPatientFeedbackDto)
            {
            var newFeedback = _feedbacksService.AddPatientFeedback(newPatientFeedbackDto);
            return Ok(newFeedback);
            }
        

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdatePatientFeedback(Guid id, UpdatePatientFeedbackDto updatePatientFeedbackDto)
            {
            var updated = _feedbacksService.UpdatePatientFeedback(id, updatePatientFeedbackDto);
            if (updated == null)
                {
                _log.LogWarning($"No feedback found with ID: {id} for update.");
                return NotFound("No feedback found for update.");
                }
            return Ok(updated);
            }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeletePatientFeedback(Guid id)
            {
            var deleted = _feedbacksService.DeletePatientFeedback(id);
            if (deleted == null)
                {
                _log.LogWarning($"No feedback found with ID: {id} for deletion.");
                return NotFound("No feedback found for deletion.");
                }
            return Ok(deleted);

            }

        }
    }
