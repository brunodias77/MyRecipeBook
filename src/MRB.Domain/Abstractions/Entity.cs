using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRB.Domain.Abstractions;

public class Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required] public bool Active { get; set; } = true;
    [Required] public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}