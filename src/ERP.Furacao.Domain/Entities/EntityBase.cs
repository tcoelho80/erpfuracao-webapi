using System.ComponentModel.DataAnnotations;

namespace ERP.Furacao.Domain.Entities
{
    public class EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }
}
