namespace HospitalAppointmentProject1.Models
{
    public interface IEmailSender
    {
        Task EmailSenderAsync(string email, string subject, string message);
    }
}
