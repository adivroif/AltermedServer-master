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
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;
        public AppointmentsController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;

            }
        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            
            return Ok(_appointmentService.GetAllAppointments());
        }



        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatientId(string patientId)
        {
            var result = await _appointmentService.GetAppointmentsByPatientId(Guid.Parse(patientId));
            return result == null || !result.Any() ? NotFound("No appointments found.") : Ok(result);
         }


        [HttpGet("{id:guid}")]
        public IActionResult GetAppointmentByUId(Guid id)
        {
            var appoitment = _appointmentService.GetAppointmentByUId(id);
            if (appoitment is null)
            {
                return NotFound();
            }
            return Ok(appoitment);
        }



        [HttpPost]
        public IActionResult AddAppointment(NewAppointmentDto dto)
        {
            var newAppointment = _appointmentService.AddAppointment(dto);
            return Ok(newAppointment);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateAppointment(Guid id, UpdateAppointmentDto dto)
        {
            var updated = _appointmentService.UpdateAppointment(id, dto);
            return updated == null ? NotFound() : Ok(updated);
            }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteAppointment(Guid id)
        {
            return _appointmentService.DeleteAppointment(id) ? Ok("Deleted.") : NotFound();
        }
    }
}
