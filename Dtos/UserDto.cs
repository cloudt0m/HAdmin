namespace HAdmin.Dtos
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string IdNumber { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserDto
    {
        public string IdNumber { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}