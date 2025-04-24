using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Services
    {

    public class AppointmentService
        {
        private readonly ApplicationDbContext _context;
        public AppointmentService(ApplicationDbContext context)
            {
            _context = context;
            }
        public List<Appointment> GetAllAppointments()
            {
            return _context.Appointments.ToList();
            }

        public async Task<List<Appointment>> GetAppointmentsByPatientId(Guid patientId)
            {
            var appointments = await _context.Appointments
                .Where(a => a.patientId == patientId)
                .ToListAsync();
            if (appointments is null || !appointments.Any())
                {
                return null;
                }
            return appointments;
            }
        public Appointment? GetAppointmentByUId(Guid id)
            {
            return _context.Appointments.Find(id);
            }

        public Appointment AddAppointment(NewAppointmentDto newAppointment)
            {
            var appointment = new Appointment
                {
                startSlot = newAppointment.startSlot,
                treatmentId = newAppointment.treatmentId,
                patientId = newAppointment.patientId,
                doctorId = newAppointment.doctorId,
                recordId = newAppointment.recordId,
                statusOfAppointment = Status.Free,
                Address = newAppointment.Address,
                duration = newAppointment.duration,
                };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return appointment;




            }
        public Appointment? UpdateAppointment(Guid id, UpdateAppointmentDto updateDto)
            {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null) return null;

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
            if (appointment == null) return false;

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return true;
            }









        }
    }
