using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentProject1.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorWorkingHours> DoctorWorkingHours { get; set; } // Add this line


        //MsSql server
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; 
Database=HospitalAppointmentProject1;Trusted_Connection=True;");

        }
    }
}
