using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class UpdateRequest
    {
        public string Username { get; set; }
        public string Address { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Avatar { get; set; }
    }
}
