using AltermedManager.Controllers;
using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Services
    {

    public class AppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddressController> _log;
        public AppointmentService(ApplicationDbContext context, ILogger<AddressController> log)
        {
            _context = context;
            _log = log;
            }

        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments
                                              .Include(a => a.Address)  
                                              .ToList();
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientId(Guid patientId)
        {
            var appointments = await _context.Appointments
                .Where(a => a.patientId == patientId)
                .ToListAsync();
            if (appointments is null || !appointments.Any())
            {
                _log.LogWarning("No appointments found for patient with ID: {PatientId}", patientId);
                return null;
            }
            return appointments;
        }

        /*
         * This method retrieves appointments for a specific doctor within the next 30 days.
         * Number of days configurable (numberOfDays)
        */
        public async Task<List<Appointment>> GetAppointmentsByDoctorId(Guid doctorId)
        {
            var numberOfDays = 30;
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var in30days = today.AddDays(numberOfDays);
            var appointments = await _context.Appointments
                .Where(a => a.doctorId == doctorId &&
                            a.startAppSlot.date_of_treatment >= today &&
                            a.startAppSlot.date_of_treatment <= in30days)
                .Include(a => a.Address)
                .Include(a => a.startAppSlot)
                .ToListAsync();

            if (appointments == null || !appointments.Any())
                {
                _log.LogWarning("No appointments found for doctor with ID: {DoctorId} in the next {Days} days", doctorId, numberOfDays);
                return null;
                }
            return appointments;
            }

        public async Task<List<Appointment>> AppointmentsByDoctorId(Guid doctorId)
        {
            var appointments = await _context.Appointments
                .Where(d => d.doctorId == doctorId)
                .Include(a => a.Address)
                .Include(a => a.startAppSlot)
                .ToListAsync();
            if (appointments is null || !appointments.Any())
            {
                _log.LogWarning("No appointments found for doctor with ID: {DoctorId}", doctorId);
                return null;
            }
            return appointments;
        }
        public Appointment? GetAppointmentByUId(Guid id)
        {
            return _context.Appointments
                .Include(a => a.Address) // load the address related to the appointment by FK
                .FirstOrDefault(a => a.appointmentId == id); // relevant appointment returned
        }

        public Appointment AddAppointment(NewAppointmentDto newAppointment)
        {
            // load the address by its ID
            if(newAppointment == null)
                {
                _log.LogError("Got empty object for appointment, new appointment not created");
                return null;
                }
            var address = _context.Address
                .FirstOrDefault(a => a.Id == newAppointment.Address.Id);

            if (address == null)
            {
                _log.LogError("Address with ID {AddressId} not found", newAppointment.Address.Id);
                return null;
            }

            //update address fields
            //address.city = newAppointment.Address.city;
            //address.street = newAppointment.Address.street;
            //address.houseNumber = newAppointment.Address.houseNumber;

            var appointment = new Appointment
            {
                startSlot = newAppointment.startSlot,
                treatmentId = newAppointment.treatmentId,
                patientId = newAppointment.patientId,
                doctorId = newAppointment.doctorId,
                recordId = newAppointment.recordId,
                statusOfAppointment = newAppointment.statusOfAppointment,
                AddressId = address.Id,
                duration = newAppointment.duration,
            };
            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return appointment;
        }

        public Appointment? UpdateAppointment(Guid id, UpdateAppointmentDto updateDto)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
                {
                _log.LogWarning("Appointment with ID {Id} not found for update", id);
                return null;
                }

            appointment.startSlot = updateDto.startSlot;
            appointment.treatmentId = updateDto.treatmentId;
            appointment.patientId = updateDto.patientId;
            appointment.doctorId = updateDto.doctorId;
            appointment.recordId = updateDto.recordId;
            appointment.statusOfAppointment = updateDto.statusOfAppointment;
            appointment.Address = updateDto.Address;
            appointment.duration = updateDto.duration;

            _context.SaveChanges();
            return appointment;
        }

        public bool DeleteAppointment(Guid id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
                {
                _log.LogWarning("Appointment with ID {Id} not found for deletion", id);
                return false;
                }

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return true;
        }

    


        public async Task<List<Appointment>> GetUnreportedAppointmentsByPatientId(Guid patientId)
            {
            var appointmentsWithoutReport = await _context.Appointments
                                                       .Where(a => a.patientId == patientId)
                                                       .Where(a => !_context.PatientFeedbacks
                                                       .Any(r => r.patientId == patientId && r.appointmentId == a.appointmentId))
                                                       .ToListAsync();
            if (appointmentsWithoutReport is null || !appointmentsWithoutReport.Any())
                {
                _log.LogWarning("No unreported appointments found for patient with ID: {PatientId}", patientId);
                return null;
                }
            return appointmentsWithoutReport;
            }
        }
}
