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
                //appointmentId = newAppointment.appointmentId,
                slotid = newAppointmentSlot.slotid,
                doctorid = newAppointmentSlot.doctorid,
                date_of_treatment = newAppointmentSlot.date_of_treatment,
                starttime = newAppointmentSlot.starttime,
                endtime = newAppointmentSlot.endtime,
                isbooked =  newAppointmentSlot.isbooked
            };

            dbContext.AppointmentSlots.Add(appointmentSlotsEntity);
            dbContext.SaveChanges();

            return Ok(appointmentSlotsEntity);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetAppointmentSlotByUId(Guid id)
        {
            var slot = dbContext.AppointmentSlots.Find(id);
            if (slot is null)
            {
                return NotFound();
            }
            return Ok(slot);
        }

    }
}
