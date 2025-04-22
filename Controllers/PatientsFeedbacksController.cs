using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
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
        private readonly ApplicationDbContext dbContext;
        private readonly TreatmentService _treatmentService;
        public PatientsFeedbacksController(ApplicationDbContext dbContext, TreatmentService treatmentService)
        {
            this.dbContext = dbContext;
            _treatmentService = treatmentService;

            }

        [HttpGet]
        public IActionResult GetAllPatientsFeedbacks()
        {
            var allPatientsFeedbacks = dbContext.PatientFeedbacks.ToList();
            return Ok(allPatientsFeedbacks);
        }



        [HttpGet("pasport{id}")]
        public async Task<IActionResult> GetPatientFeedbackById(Guid id)
        {
            var feedback = await dbContext.PatientFeedbacks
                .Where(f => f.feedbackId == id)
                .FirstOrDefaultAsync();
            if (feedback is null)
            {
                return NotFound();
            }
            return Ok(feedback);
        }


        [HttpGet("{id:guid}")]
        public IActionResult GetPatientFeedbackByUId(Guid id)
        {
            var feedback = dbContext.PatientFeedbacks.Find(id);
            if (feedback is null)
            {
                return NotFound();
            }
            return Ok(feedback);
        }



        [HttpPost]
        public IActionResult AddPatientFeedback(NewPatientFeedbackDto newPatientFeedback)
        {
            var patientFeedbackEntity = new PatientFeedback()
            {
                patientId = newPatientFeedback.patientId,
                appointmentId = newPatientFeedback.appointmentId,
                overallStatus = newPatientFeedback.overallStatus,
                comments = newPatientFeedback.comments,
                createdOn = newPatientFeedback.createdOn,
                newSymptoms = newPatientFeedback.newSymptoms
            };
            //dbContext.PatientFeedbacks.Add(patientFeedbackEntity);
            //dbContext.SaveChanges();

            _treatmentService.UpdateTreatmentScore(newPatientFeedback.overallStatus, newPatientFeedback.appointmentId);
            return Ok(patientFeedbackEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdatePatientFeedback(Guid id, UpdatePatientFeedbackDto updatePatientFeedbackDto)
        {
            var feedback = dbContext.PatientFeedbacks.Find(id);
            if (feedback is null)
            {
                return NotFound();
            }
            feedback.patientId = updatePatientFeedbackDto.patientId;
            feedback.appointmentId = updatePatientFeedbackDto.appointmentId;
            feedback.overallStatus = updatePatientFeedbackDto.overallStatus;
            feedback.comments = updatePatientFeedbackDto.comments;
            feedback.createdOn = updatePatientFeedbackDto.createdOn;
            feedback.newSymptoms = updatePatientFeedbackDto.newSymptoms;

            dbContext.SaveChanges();
            return Ok(feedback);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeletePatientFeedback(Guid id)
        {
            var feedback = dbContext.PatientFeedbacks.Find(id);
            if(feedback is null)
            {
                return NotFound();
            }
            dbContext.PatientFeedbacks.Remove(feedback);
            dbContext.SaveChanges();
            return Ok("Patient deleted");
        }
    }
}
