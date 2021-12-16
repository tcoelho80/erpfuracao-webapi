using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ERP.Furacao.Application.DTOs.Account
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [JsonIgnore]
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [JsonIgnore]
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
