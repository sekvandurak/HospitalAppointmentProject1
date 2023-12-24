using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentProject1.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Patient> Patients => Set<Patient>();

        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<DoctorWorkingHours> DoctorWorkingHours => Set<DoctorWorkingHours>(); // Add this line
        public DbSet<User> Users => Set<User>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
