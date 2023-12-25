using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentProject1.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<DoctorWorkingHours> DoctorWorkingHours => Set<DoctorWorkingHours>();



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
