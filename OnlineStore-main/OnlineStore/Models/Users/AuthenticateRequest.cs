using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models.Users
{
    public class AuthenticateRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}