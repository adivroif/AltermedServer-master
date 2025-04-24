using AltermedManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientRequest> PatientRequest { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentSlots> AppointmentSlots { get; set; }


        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedule { get; set; }

        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<PatientFeedback> PatientFeedbacks { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }

        }
    }
