using System.ComponentModel.DataAnnotations;

namespace ERP.Furacao.Application.DTOs
{
    public class EntityBaseDTO
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }
}
