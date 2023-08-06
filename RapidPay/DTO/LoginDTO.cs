using Domain;

namespace RapidPay.DTO
{
    public class LoginRequestDTO
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string? Token { get; set; }
    }
}
