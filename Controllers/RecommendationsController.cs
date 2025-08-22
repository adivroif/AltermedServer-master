using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly RecommendationService _recommendationService;
        private readonly ILogger<AddressController> _log;

        //for test - REMOVEEEEE
        private readonly TreatmentService _treatmentService;
        public RecommendationsController(RecommendationService recommendationService, TreatmentService treatmentService, ILogger<AddressController> log)
            {
            _recommendationService = recommendationService;
            _treatmentService = treatmentService;
            _log = log;
            }

        [HttpGet("byAppoint/{appointmentId}")] //route parameter
        //change to query param for testing

        public async Task<IActionResult> GetRecommendationByAppointmentId([DefaultValue("01968274-f546-7ab7-a71e-6c5eb35f2a4e")] Guid appointmentId)
            {
            var recommendation = await _recommendationService.GetRecommendationsByTreatmentGroupAsync(appointmentId);
            if (recommendation is null)
                {
                _log.LogWarning($"No recommendation found for appointment ID: {appointmentId}");
                return NotFound();
                }
            return Ok(recommendation);
            }

        [HttpGet("byPatient/{PatientId}")] //route parameter
        //change to query param for testing
        public async Task<IActionResult> GetRecommendationsOfPatient(Guid patientId)
        {
            var result = await _recommendationService.GetRecommendationsOfPatient(patientId);
            if (result == null)
                {
                _log.LogWarning($"No recommendations found for patient ID: {patientId}");
                return NotFound("No recommendations found.");
                }
            return Ok(result);
        }

        [HttpGet("byRecommendationId/{recommendationId}")] //route parameter
        //change to query param for testing
        public async Task<IActionResult> GetRecommendation(int recommendationId)
        {
            var result = await _recommendationService.GetRecommendation(recommendationId);
            if (result == null)
                {
                _log.LogWarning($"No recommendation found with ID: {recommendationId}");
                return NotFound("No recommendation found.");
                }
            return Ok(result);
        }

        [HttpGet("byPatient/ischosen/{PatientId}")] //route parameter
        //change to query param for testing
        public async Task<IActionResult> GetRecommendationsNotChosenOfPatient(Guid patientId)
        {
            var result = await _recommendationService.GetRecommendationsNotChosenOfPatient(patientId);
            if (result == null || !result.Any())
                {
                _log.LogWarning($"Not no chosen found for patient ID: {patientId}");
                return NotFound("No recommendations not chosen found.");
                }
            return Ok(result);
        }


        //this end point used for testing purposes only
        [HttpGet("{bodyPart}/{treatmentId}")]
        public IActionResult testEndPoint(string bodyPart, int treatmentId)
            {
            var treatment = _treatmentService.GetTreatmentByUId(treatmentId);
            var result = _recommendationService.FindTreatmentByBodyPart(bodyPart, treatment);
            return result == null ? NotFound("No feedback found.") : Ok(result);
            }

        [HttpPut]
        [Route("recommendationId/{recommendationId}")]
        public IActionResult UpdateRecommendation(int recommendationId, UpdateRecommendationDto updateRecommendationDto)
        {
            var result = _recommendationService.UpdateRecommendation(recommendationId, updateRecommendationDto);
            if (result == null)
                {
                _log.LogWarning($"Failed to update recommendation with ID: {recommendationId}");
                return NotFound("Failed in update recommendation.");
                }
            return Ok(result);
        }


    }
}
