using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineStore.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public Role Role { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        [JsonIgnore]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Mật khẩu ít nhất 8 ký tự.", MinimumLength = 8)]
        public string PasswordHash { get; set; }

    }
}