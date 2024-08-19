using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MRB.Domain.Abstractions;
using MRB.Domain.Enums;

namespace MRB.Domain.Entities;

public class Recipe : Entity
{
    [Required] [MaxLength(255)] 
    public string Title { get; set; }

    public int? CookingTime { get; set; }

    public int? Difficulty { get; set; }
    
    public string? ImageIdentifier { get; set; }

    [Required] public Guid UserId { get; set; }

    [ForeignKey("UserId")] public User User { get; set; }

    public ICollection<Instruction> Instructions { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }
    public ICollection<DishType> DishTypes { get; set; }

    public Recipe()
    {
    }
    
}


// public string Title { get; set; } = String.Empty;
// public CookingTime? CookingTime { get; set; }
// public Difficulty? Difficulty { get; set; }
//
// public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
// public ICollection<Instruction> Instructions { get; set; } = new List<Instruction>();
// public ICollection<DishType> DishTypes { get; set; } = new List<DishType>();
//
// // Chave estrangeira para o usuário
// public Guid UserId { get; set; }
//
// public Recipe()
// {
// }