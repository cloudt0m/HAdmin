using System;

namespace HAdmin.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class UserOutput
    {
        public Guid Id { get; set; }
        public string IdNumber { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }


    }
}