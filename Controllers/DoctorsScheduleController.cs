using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsScheduleController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<AddressController> _log;
        public DoctorsScheduleController(ApplicationDbContext dbContext, ILogger<AddressController> log)
        {
            this.dbContext = dbContext;
            _log = log;
        }
        [HttpGet]
        public IActionResult GetAllDoctorsSchedule()
        {
            var allDoctorsSchedule = dbContext.DoctorSchedule
                                              .Include(ds => ds.Address)  
                                              .ToList();
            return Ok(allDoctorsSchedule);
        }


        [HttpGet("{id:guid}")]
        public IActionResult GetDoctorSchedulesByUId(Guid id)
        {
            var schedules = dbContext.DoctorSchedule
                                     .Include(ds => ds.Address) 
                                     .Where(ds => ds.doctorid == id.ToString()) 
                                     .ToList();

            if (schedules == null || !schedules.Any())
            {
                _log.LogWarning($"No schedules found for doctor with ID: {id}");
                return NotFound(); 
            }

            return Ok(schedules); 
        }


        [HttpPost]
        public IActionResult? AddDoctorSchedule(NewDoctorScheduleDto newDoctorSchedule)
        {
            var address = dbContext.Address.Find(newDoctorSchedule.Address.Id);
            if (address == null)
            {
                _log.LogError($"Address with ID {newDoctorSchedule.Address.Id} not found for new doctor schedule.");
                return null;
            }
            var doctorScheduleEntity = new DoctorSchedule()
            {
                scheduleid = newDoctorSchedule.scheduleid,
                doctorid = newDoctorSchedule.doctorid,
                date = newDoctorSchedule.date,
                starttime = newDoctorSchedule.starttime,
                endtime = newDoctorSchedule.endtime,
                slotsid = newDoctorSchedule.slotsid,
                Address = address,
            };
            dbContext.DoctorSchedule.Add(doctorScheduleEntity);
            dbContext.SaveChanges();

            return Ok(doctorScheduleEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateDoctor(Guid id, UpdateDoctorScheduleDto updateDoctorScheduleDto)
        {
            var doctorSchedule = dbContext.DoctorSchedule.Find(id);
            if (doctorSchedule is null)
            {
               _log.LogWarning($"Doctor schedule with ID {id} not found for update.");
                return NotFound();
            }
            doctorSchedule.scheduleid = updateDoctorScheduleDto.scheduleid;
            doctorSchedule.doctorid = updateDoctorScheduleDto.doctorid;
            doctorSchedule.date = updateDoctorScheduleDto.date;
            doctorSchedule.starttime = updateDoctorScheduleDto.starttime;
            doctorSchedule.endtime = updateDoctorScheduleDto.endtime;
            doctorSchedule.slotsid = updateDoctorScheduleDto.slotsid;
            doctorSchedule.Address = updateDoctorScheduleDto.Address;

            dbContext.SaveChanges();
            return Ok(doctorSchedule);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteDoctorSchedule(Guid id)
        {
            var doctorSchedule = dbContext.DoctorSchedule.Find(id);
            if(doctorSchedule is null)
            {
               _log.LogWarning($"Doctor schedule with ID {id} not found for deletion.");
                return NotFound();
            }
            dbContext.DoctorSchedule.Remove(doctorSchedule);
            dbContext.SaveChanges();
            return Ok("Doctor schedule deleted");
        }
    }
}
