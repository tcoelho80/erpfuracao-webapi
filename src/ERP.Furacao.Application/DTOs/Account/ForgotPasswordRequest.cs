using System.ComponentModel.DataAnnotations;

namespace ERP.Furacao.Application.DTOs.Account
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
