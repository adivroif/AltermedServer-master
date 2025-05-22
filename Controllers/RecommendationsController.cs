using AltermedManager.Models.Entities;
using AltermedManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly RecommendationService _recommendationService;

        //for test - REMOVEEEEE
        private readonly TreatmentService _treatmentService;
        public RecommendationsController(RecommendationService recommendationService, TreatmentService treatmentService)
            {
            _recommendationService = recommendationService;
            _treatmentService = treatmentService;
            }

        [HttpGet("byAppoint/{appointmentId}")] //route parameter
        //change to query param for testing

        public async Task<IActionResult> GetRecommendationByAppointmentId([DefaultValue("0195a9f7-f04b-7b77-8c05-be3d51536903")] Guid appointmentId)
            {
            var recommendation = _recommendationService.GetRecommendationsByTreatmentGroup(appointmentId);
            if (recommendation is null)
                {
                return NotFound();
                }
            return Ok(recommendation);
            }

        [HttpGet("byPatient/{PatientId}")] //route parameter
        //change to query param for testing
        public async Task<IActionResult> GetRecommendationsOfPatient(Guid patientId)
        {
            var result = await _recommendationService.GetRecommendationsOfPatient(patientId);
            return result == null || !result.Any() ? NotFound("No recommendations found.") : Ok(result);
        }

        [HttpGet("byPatient/ischosen/{PatientId}")] //route parameter
        //change to query param for testing
        public async Task<IActionResult> GetRecommendationsNotChosenOfPatient(Guid patientId)
        {
            var result = await _recommendationService.GetRecommendationsNotChosenOfPatient(patientId);
            return result == null || !result.Any() ? NotFound("No recommendations not chosen found.") : Ok(result);
        }

        [HttpGet("{bodyPart}/{treatmentId}")]
        public IActionResult testEndPoint(string bodyPart, int treatmentId)
            {
            var treatment = _treatmentService.GetTreatmentByUId(treatmentId);
            var result = _recommendationService.FindTreatmentByBodyPart(bodyPart, treatment);
            return result == null ? NotFound("No feedback found.") : Ok(result);
            }


        }
}
