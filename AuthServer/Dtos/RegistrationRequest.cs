using System.ComponentModel.DataAnnotations;

namespace AuthServer.Dtos
{
    public class RegistrationRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set;} = string.Empty;

        [Required]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; } = string.Empty;
    }
}
