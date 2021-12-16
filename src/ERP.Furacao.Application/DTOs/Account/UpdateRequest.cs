using System.ComponentModel.DataAnnotations;

namespace ERP.Furacao.Application.DTOs.Account
{
    public class UpdateRequest : RegisterRequest
    {
        [Required]
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
    }
}
