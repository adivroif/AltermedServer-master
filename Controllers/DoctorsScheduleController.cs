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
        public DoctorsScheduleController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        [HttpGet]
        public IActionResult GetAllDoctorsSchedule()
        {
            var allDoctorsSchedule = dbContext.DoctorSchedule
                                              .Include(ds => ds.Address)  // טוען את הכתובת גם
                                              .ToList();
            return Ok(allDoctorsSchedule);
        }


        [HttpGet("{id:guid}")]
        public IActionResult GetDoctorSchedulesByUId(Guid id)
        {
            var schedules = dbContext.DoctorSchedule
                                     .Include(ds => ds.Address) // כולל את הכתובת
                                     .Where(ds => ds.doctorid == id.ToString()) // מסנן לפי doctorId
                                     .ToList(); // מחזיר רשימה

            if (schedules == null || !schedules.Any())
            {
                return NotFound(); // אם לא נמצאו לוחות שנה
            }

            return Ok(schedules); // מחזיר את הרשימה
        }


        [HttpPost]
        public IActionResult? AddDoctorSchedule(NewDoctorScheduleDto newDoctorSchedule)
        {
            Address address = dbContext.Address.Find(newDoctorSchedule.Address.Id);
            if (address == null)
            {
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
                return NotFound();
            }
            dbContext.DoctorSchedule.Remove(doctorSchedule);
            dbContext.SaveChanges();
            return Ok("Doctor schedule deleted");
        }
    }
}
