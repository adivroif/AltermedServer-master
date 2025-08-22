using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using AltermedManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentController : ControllerBase
    {
        private readonly TreatmentService _treatmentService;
        public TreatmentController(TreatmentService treatmentService)
        {
            _treatmentService = treatmentService;

            }
        [HttpGet]
        public IActionResult GetAllTreatments()
        {
            return Ok(_treatmentService.GetAllTreatments());
        }


        /*
        [HttpGet("passport{id}")]
        public async Task<IActionResult> GetTreatmentById(int id)   
            {
            var treatment = await _treatmentService.GetTreatmentById(id);

            if (treatment is null)
                {
                return NotFound();
                }

            return Ok(treatment);
            }*/


        [HttpGet("{id:int}")]
        public IActionResult GetTreatmentByUId(int id)
        {
            var treatment = _treatmentService.GetTreatmentByUId(id);
            if (treatment is null)
                {
                return NotFound();
                }
            return Ok(treatment);
        }

        [HttpGet("{name}")]
        public IActionResult GetTreatmentByUName(string name)
        {
            var treatment = _treatmentService.GetTreatmentByUName(name);
            if (treatment == null)
            {
                return NotFound();
            }
            return Ok(treatment);
        }
        /*
        [HttpGet("{name}")]
        public IActionResult GetTreatmentByUName(string name)
        {
            var treatment = dbContext.Treatments.FirstOrDefault(t => t.treatmentName == name);
            if (treatment == null)
            {
                return NotFound();
            }
            return Ok(treatment);
        }
        */



        [HttpPost]
        public IActionResult AddTreatment(NewTreatmentDto treatmentDto)
        {
            var newTreatment = _treatmentService.AddTreatment(treatmentDto);
            return Ok(newTreatment);
            }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateTreatment(Guid id, UpdateTreatmentDto updateTreatmentDto)
        {
            var treatment = _treatmentService.UpdateTreatment(id, updateTreatmentDto);
            return treatment is null ? NotFound() : Ok(treatment);
            }


        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteTreatment(int id)
        {
            return _treatmentService.DeleteTreatment(id) ? Ok() : NotFound();
            }
    }
}
