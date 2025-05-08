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


        public PatientsFeedbacksController(PatientsFeedbacksService feedbacksService)
            {
            _feedbacksService = feedbacksService;
            }

        [HttpGet]
        public IActionResult GetAllPatientsFeedbacks()
            {
            var result = _feedbacksService.GetAllPatientsFeedbacks();

            return result == null ? NotFound("No feedbacks found.") : Ok(result);
            }



        [HttpGet("patientId{id}")]
        public async Task<IActionResult> GetPatientFeedbackById(Guid id)
            {
            var result = await _feedbacksService.GetFeedbacksByPatientId(id);
            return result == null ? NotFound("No feedback found.") : Ok(result);
            }


        [HttpGet("{id:guid}")]
        public IActionResult GetPatientFeedbackByUId(Guid id)
            {
            var feedback = _feedbacksService.GetFeedbackByUId(id);
            if (feedback is null)
                {
                return NotFound();
                }
            return Ok(feedback);
            }

        [HttpGet("appointmentId/{appointmentId}")]
        public async Task<IActionResult> GetPatientFeedbackByAppointmentId(Guid appointmentId)
        {
            var feedback = await dbContext.PatientFeedbacks
                .Where(f => f.appointmentId == appointmentId)
                .FirstOrDefaultAsync();
            if (feedback is null)
            {
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
            return updated == null ? NotFound("No feedback found.") : Ok(updated);
            }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeletePatientFeedback(Guid id)
            {
            var deleted = _feedbacksService.DeletePatientFeedback(id);
            return deleted == null ? NotFound("No feedback found.") : Ok(deleted);

            }

        }
    }
