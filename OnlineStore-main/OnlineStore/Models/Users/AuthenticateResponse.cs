using OnlineStore.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models.Users
{
    public class AuthenticateResponse
    {
        //public int Id { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Username { get; set; }
        //public Role Role { get; set; }
        //public string Token { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public Role Role { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Birthdate = user.Birthdate;
            Address = user.Address;
            Avatar = user.Avatar;
            Role = user.Role;
            Token = token;
        }
    }
}