using AltermedManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly RecommendationService _recommendationService;
        public RecommendationsController(RecommendationService recommendationService)
            {
            _recommendationService = recommendationService;
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
        

        }
}
