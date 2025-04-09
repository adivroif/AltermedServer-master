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
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public AppointmentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            var allAppointments = dbContext.Appointments.ToList();
            return Ok(allAppointments);
        }



        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatientId(string patientId)
        {
            var appointments = await dbContext.Appointments
                .Where(a => a.patientId.ToString() == patientId)
                .ToListAsync();
            if (appointments is null || !appointments.Any())
            {
                return NotFound(new { message = "No appointments found for this patient." });
            }
            return Ok(appointments);
        }


        [HttpGet("{id:guid}")]
        public IActionResult GetAppointmentByUId(Guid id)
        {
            var appoitment = dbContext.Appointments.Find(id);
            if (appoitment is null)
            {
                return NotFound();
            }
            return Ok(appoitment);
        }



        [HttpPost]
        public IActionResult AddAppointment(NewAppointmentDto newAppointment)
        {
            var appointmentEntity = new Appointment()
            {
                //appointmentId = newAppointment.appointmentId,
                startSlot = newAppointment.startSlot,
                treatmentId = newAppointment.treatmentId,
                patientId = newAppointment.patientId,
                doctorId = newAppointment.doctorId,
                recordId = newAppointment.recordId,
                //place    = newAppointment.place,
                statusOfAppointment =  Status.Free
            };

            dbContext.Appointments.Add(appointmentEntity);
            dbContext.SaveChanges();

            return Ok(appointmentEntity);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateAppointment(Guid id, UpdateAppointmentDto updateAppointmentDto)
        {
            var appointment
                = dbContext.Appointments.Find(id);
            if (appointment is null)
            {
                return NotFound();
            }
            appointment.startSlot = updateAppointmentDto.startSlot;
            appointment.treatmentId = updateAppointmentDto.treatmentId;
            appointment.patientId = updateAppointmentDto.patientId;
            appointment.doctorId = updateAppointmentDto.doctorId;
            appointment.recordId = updateAppointmentDto.recordId;
            //appointment.place = updateAppointmentDto.place;
            appointment.statusOfAppointment = updateAppointmentDto.statusOfAppointment;
            dbContext.SaveChanges();
            return Ok(appointment);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteAppointment(Guid id)
        {
            var appointment = dbContext.Appointments.Find(id);
            if (appointment is null)
            {
                return NotFound();
            }
            dbContext.Appointments.Remove(appointment);
            dbContext.SaveChanges();
            return Ok("Appointment  deleted");
        }
    }
}
