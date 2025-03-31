using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TretmentController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public TretmentController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        [HttpGet]
        public IActionResult GetAllTreatments()
        {
            var allTreatments = dbContext.Treatments.ToList();
            return Ok(allTreatments);
        }



        [HttpGet("pasport{id}")]
        public async Task<IActionResult> GetTreatmentById(int id)
        {
            var Treatment = await dbContext.Treatments
                .Where(t => t.treatmentId == id)
                .FirstOrDefaultAsync();
            if (Treatment is null)
            {
                return NotFound();
            }
            return Ok(Treatment);
        }


        [HttpGet("{id:guid}")]
        public IActionResult GetTreatmentByUId(int id)
        {
            var Treatment = dbContext.Treatments.Find(id);
            if (Treatment is null)
            {
                return NotFound();
            }
            return Ok(Treatment);
        }



        [HttpPost]
        public IActionResult AddTreatment(NewTreatmentDto newTreatment)
        {
            var treatmentEntity = new Treatment()
            {
                treatmentId = newTreatment.treatmentId,
                treatmetName = newTreatment.treatmetName,
                treatmentDescription = newTreatment.treatmentDescription,
               // treatmentPlace = newTreatment.treatmentPlace,
                treatmentPrice = newTreatment.treatmentPrice,
                SuitCategories = newTreatment.SuitCategories,
            };
            dbContext.Treatments.Add(treatmentEntity);
            dbContext.SaveChanges();

            return Ok(treatmentEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateTreatment(Guid id, UpdateTreatmentDto updateTreatmentDto)
        {
            var treatment = dbContext.Treatments.Find(id);
            if (treatment is null)
            {
                return NotFound();
            }
            treatment.treatmentId = updateTreatmentDto.treatmentId;
            treatment.treatmetName = updateTreatmentDto.treatmetName;
            treatment.treatmentPrice = updateTreatmentDto.treatmentPrice;
           // treatment.treatmentPlace = updateTreatmentDto.treatmentPlace;
            treatment.treatmentDescription = updateTreatmentDto.treatmentDescription;
            treatment.SuitCategories = updateTreatmentDto.SuitCategories;

            dbContext.SaveChanges();
            return Ok(treatment);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteTreatment(Guid id)
        {
            var treatment = dbContext.Treatments.Find(id);
            if(treatment is null)
            {
                return NotFound();
            }
            dbContext.Treatments.Remove(treatment);
            dbContext.SaveChanges();
            return Ok("Treatment deleted");
        }
    }
}
