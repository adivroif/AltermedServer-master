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
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<AddressController> _log;
        public DoctorsController(ApplicationDbContext dbContext, ILogger<AddressController> log)
        {
            this.dbContext = dbContext;
            _log = log;

        }
        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var allDoctors = dbContext.Doctors.ToList();
            return Ok(allDoctors);
        }


        [HttpGet("{id:guid}")]
        public IActionResult GetDoctorByUId(Guid id)
        {
            var patient = dbContext.Doctors.Find(id);
            if (patient is null)
            {
                _log.LogWarning("Doctor with {Id} not found.", id);
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpGet("{name}")]
        public IActionResult GetDoctorByUName(string name)
        {
            var doctor = dbContext.Doctors.FirstOrDefault(d => d.doctorName + " " + d.doctorSurname == name);
            if (doctor == null)
            {
                _log.LogWarning("Doctor with {Name} not found.", name);
                return NotFound();
            }
            return Ok(doctor);
        }

        [HttpGet("doctorId/{doctorId}")]
        public async Task<IActionResult> GetDoctorTreatments(string doctorId)
        {
            try
                {
                var doctor = await dbContext.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorId.ToString() == doctorId);

                if (doctor == null)
                    {
                    _log.LogWarning("No treatments returned - Doctor with {Id} not found", doctorId);
                    return NotFound("Doctor not found");
                    }
                var treatments = await dbContext.Treatments
                    .Where(t => doctor.specList.Contains(t.treatmentName))
                    .ToListAsync();
                if (treatments.Count == 0)
                    {
                    _log.LogWarning("Doctor {DoctorId} specialties matched 0 treatments", doctorId);
                    }
                else
                    {
                    ;
                    }
                return Ok(treatments);
                }
                catch (Exception e)
                {
                _log.LogError(e, "Error getting treatments for doctor {DoctorId}", doctorId);
                return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost]
        public IActionResult AddDoctor(NewDoctorDto newDoctor)
        {
            var doctorEntity = new Doctor()
            {
                DoctorId = newDoctor.DoctorId,
                doctorLicense = newDoctor.doctorLicense,
                doctorName = newDoctor.doctorName,
                doctorSurname = newDoctor.doctorSurname,
                specList = newDoctor.specList,
                scheduleId = newDoctor.scheduleId, 
                placesWorking = newDoctor.placesWorking,
                Email = newDoctor.Email,
                Phone = newDoctor.Phone
            };
            dbContext.Doctors.Add(doctorEntity);
            dbContext.SaveChanges();

            return Ok(doctorEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateDoctor(Guid id, UpdateDoctorDto updateDoctorDto)
        {
            var doctor = dbContext.Doctors.Find(id);
            if (doctor is null)
            {
                _log.LogWarning("Doctor with {Id} not found.", id);
                return NotFound();
            }
            //doctor.DoctorId = updateDoctorDto.DoctorId;
            doctor.doctorName = updateDoctorDto.doctorName;
            doctor.doctorSurname = updateDoctorDto.doctorSurname;
            doctor.doctorLicense = updateDoctorDto.doctorLicense;
            if(updateDoctorDto.specList !=null)
                doctor.specList = updateDoctorDto.specList;
            
            doctor.scheduleId = updateDoctorDto.scheduleId;
            doctor.placesWorking = updateDoctorDto.placesWorking;
            doctor.Email = updateDoctorDto.Email;
            doctor.Phone = updateDoctorDto.Phone;

            dbContext.SaveChanges();
            return Ok(doctor);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteDoctor(Guid id)
        {
            var doctor = dbContext.Doctors.Find(id);
            if(doctor is null)
            {
                _log.LogWarning("Doctor with {Id} not found", id);
                return NotFound();
            }
            dbContext.Doctors.Remove(doctor);
            dbContext.SaveChanges();
            return Ok("Doctor deleted");
        }
    }
}
