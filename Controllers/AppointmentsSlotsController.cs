using System.Globalization;
using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsSlotsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public AppointmentsSlotsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        [HttpGet]
        public IActionResult GetAllAppointmentsSlots()
        {
            var allAppointmentsSlots = dbContext.AppointmentSlots.ToList();
            return Ok(allAppointmentsSlots);
        }


        [HttpPost]
        public IActionResult AddAppointmentSlot(NewAppointmentSlotsDto newAppointmentSlot)
        {
            var appointmentSlotsEntity = new AppointmentSlots()
            {

                doctorid = newAppointmentSlot.doctorid,
                date_of_treatment = newAppointmentSlot.date_of_treatment,
                starttime = newAppointmentSlot.starttime,
                endtime = newAppointmentSlot.endtime,
                isbooked = newAppointmentSlot.isbooked,
            };

            dbContext.AppointmentSlots.Add(appointmentSlotsEntity);
            dbContext.SaveChanges();

            return Ok(appointmentSlotsEntity);
        }

        [HttpPut("{id:int}")] 
        public IActionResult UpdateAppointmentSlot(int id, NewAppointmentSlotsDto updatedSlot)
        {
            var existingSlot = dbContext.AppointmentSlots.Find(id);
            if (existingSlot == null)
            {
                return NotFound(); 
            }
            Console.WriteLine(existingSlot);
            existingSlot.doctorid = updatedSlot.doctorid;
            existingSlot.date_of_treatment = updatedSlot.date_of_treatment;
            existingSlot.starttime = updatedSlot.starttime;
            existingSlot.endtime = updatedSlot.endtime;
            existingSlot.isbooked = updatedSlot.isbooked;

            dbContext.SaveChanges();
            return Ok(existingSlot);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetAppointmentSlotByUId(int id)
        {
            var slot = dbContext.AppointmentSlots.Find(id);
            if (slot is null)
            {
                return NotFound();
            }
            return Ok(slot);
        }

        [HttpGet("{startTime}/{doctorId}/{date}")]
        public IActionResult GetSpecificAppointmentSlotByUId(DateOnly date ,Guid doctorId,TimeOnly startTime)
        {

            Console.WriteLine(startTime);
            Console.WriteLine(doctorId);
            Console.WriteLine(date);


            var slot = dbContext.AppointmentSlots
            .Where(s => s.starttime == startTime.ToString() && 
                        s.doctorid == doctorId && s.date_of_treatment.Month == date.Month && s.date_of_treatment.Year == date.Year && s.date_of_treatment.DayNumber == date.DayNumber).FirstOrDefault();                         
            if (slot is null)
            {
                return NotFound("Slot not found.");
            }
            return Ok(slot);
        }

        

        [HttpDelete]
        [Route("{id:int}")]
        public bool DeleteAppointmentSlot(int id)
        {
            var appointmentSlot = dbContext.AppointmentSlots.Find(id);
            if (appointmentSlot == null) return false;

            dbContext.AppointmentSlots.Remove(appointmentSlot);
            dbContext.SaveChanges();
            return true;
        }

    }
}
