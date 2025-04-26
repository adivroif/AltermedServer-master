using AltermedManager.Data;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments
                                              .Include(a => a.Address)  // טוען את הכתובת גם
                                              .ToList();
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
            return _context.Appointments
                .Include(a => a.Address) // טוען גם את הכתובת לפי ה-FK
                .FirstOrDefault(a => a.appointmentId == id); // מחזיר את הפגישה המתאימה
        }


        public Address? GetAddressByAddressId(int id)
        {
            return _context.Address.Find(id);
        }

        public Appointment AddAppointment(NewAppointmentDto newAppointment)
        {
            // 1. טוענים את הכתובת הקיימת לפי ID
            var address = _context.Address
                .FirstOrDefault(a => a.Id == newAppointment.Address.Id);

            if (address == null)
            {
                throw new Exception("Address not found"); // לא אמור לקרות אם אתה שולח id קיים
            }

            // 2. מעדכנים את השדות בכתובת
            address.city = newAppointment.Address.city;
            address.street = newAppointment.Address.street;
            address.houseNumber = newAppointment.Address.houseNumber;

            // 3. אין צורך לעשות _context.Add(address); כי הוא כבר קיים

            // 4. יוצרים את ה-Appointment ומפנים רק ל-id של הכתובת
            var appointment = new Appointment
            {
                startSlot = newAppointment.startSlot,
                treatmentId = newAppointment.treatmentId,
                patientId = newAppointment.patientId,
                doctorId = newAppointment.doctorId,
                recordId = newAppointment.recordId,
                statusOfAppointment = newAppointment.statusOfAppointment,
                AddressId = address.Id, // קישור לפי id
                duration = newAppointment.duration,
            };

            _context.Appointments.Add(appointment);

            // 5. שומרים גם את העדכון של הכתובת וגם את הפגישה
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
