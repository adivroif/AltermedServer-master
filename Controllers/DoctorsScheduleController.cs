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
    public class DoctorsScheduleController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public DoctorsScheduleController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        [HttpGet]
        public IActionResult GetAllDoctorsSchedule()
        {
            var allDoctorsSchedule = dbContext.DoctorSchedule.ToList();
            return Ok(allDoctorsSchedule);
        }


        [HttpGet("{id:guid}")]
        public IActionResult GetDoctorScheduleByUId(Guid id)
        {
            var schedule = dbContext.DoctorSchedule.Find(id);
            if (schedule is null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpPost]
        public IActionResult AddDoctorSchedule(NewDoctorScheduleDto newDoctorSchedule)
        {
            var doctorScheduleEntity = new DoctorSchedule()
            {
                scheduleid = newDoctorSchedule.scheduleid,
                doctorid = newDoctorSchedule.doctorid,
                date = newDoctorSchedule.date,
                starttime = newDoctorSchedule.starttime,
                endtime = newDoctorSchedule.endtime,
                slotid = newDoctorSchedule.slotid, 
                address = newDoctorSchedule.address,
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
                return NotFound();
            }
            doctorSchedule.scheduleid = updateDoctorScheduleDto.scheduleid;
            doctorSchedule.doctorid = updateDoctorScheduleDto.doctorid;
            doctorSchedule.date = updateDoctorScheduleDto.date;
            doctorSchedule.starttime = updateDoctorScheduleDto.starttime;
            doctorSchedule.endtime = updateDoctorScheduleDto.endtime;
            doctorSchedule.slotid = updateDoctorScheduleDto.slotid;
            doctorSchedule.address = updateDoctorScheduleDto.address;

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
                return NotFound();
            }
            dbContext.DoctorSchedule.Remove(doctorSchedule);
            dbContext.SaveChanges();
            return Ok("Doctor schedule deleted");
        }
    }
}
