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
        private readonly ILogger<AddressController> _log;
        public AppointmentsController(AppointmentService appointmentService, ILogger<AddressController> log)
            {
            _appointmentService = appointmentService;
            _log = log;
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
            if(result == null || !result.Any())
                {
                _log.LogInformation("Appointments for patient with {Id} not found.", patientId);
                NotFound("No appointments found.");
                }
            return Ok(result);
            }


        /*GET endpoint to get all appointment of specific doctor in range of 30 days*/
        [HttpGet("doctor30/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctorId(string doctorId)
        {
            var result = await _appointmentService.GetAppointmentsByDoctorId(Guid.Parse(doctorId));
            if (result == null || result.Count == 0)
                {
                _log.LogInformation("Appointments to next 30 days with doctor with {Id} not found.", doctorId);
                NotFound("No appointments in range of 30 days found.");
                }
            return Ok(result);

        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> AppointmentsByDoctorId(string doctorId)
        {
            var result = await _appointmentService.AppointmentsByDoctorId(Guid.Parse(doctorId));
            if (result == null || result.Count == 0)
                {
                _log.LogInformation("Appointments with doctor with {Id} not found.", doctorId);
                NotFound("No appointments  found.");
                }
            return Ok(result);
            }

        [HttpGet("{id:guid}")]
        public IActionResult GetAppointmentByUId(Guid id)
            {
            var appointment = _appointmentService.GetAppointmentByUId(id);
            if (appointment is null)
                {
                _log.LogInformation("Appointments with {Id} not found.", id);
                return NotFound();
                }
            return Ok(appointment);
            }



        [HttpPost]
        public IActionResult AddAppointment(NewAppointmentDto dto)
            {
            var newAppointment = _appointmentService.AddAppointment(dto);
            if (newAppointment is null)
                {
                _log.LogWarning("Appointment with {Id} not created. Check input data.", dto.appointmentId);
                return BadRequest("Appointment not created. Check input data.");
                }
            return Ok(newAppointment);
            }


        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateAppointment(Guid id, UpdateAppointmentDto dto)
            {
            var updated = _appointmentService.UpdateAppointment(id, dto);
            if(updated == null)
                {
                _log.LogWarning("Appointment wit {Id} not found. Not updated", id);
                return NotFound();
                }
            return Ok(updated);
            }


        [HttpDelete("{id:guid}")]
        public IActionResult DeleteAppointment(Guid id)
            {
            if (_appointmentService.DeleteAppointment(id))
                {
                _log.LogInformation("Appointment {AppointmentId} deleted", id);
                return Ok("Deleted.");
                }
            else
                {
                _log.LogWarning("Delete requested for non-existing appointment {AppointmentId}", id);
                return NotFound();
                }
            }

        }
    }
