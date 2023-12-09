using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentProject1.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        //MsSql server
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; 
Database=HospitalAppointmentProject1;Trusted_Connection=True;");
        }
    }
}
