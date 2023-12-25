using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.ViewModels
{
    public class EditViewModel
    {
        public string? Id { get; set; }

        public string? FullName { get; set; }


        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public IList<string>? SelectedRoles { get; set; }
    }
}
