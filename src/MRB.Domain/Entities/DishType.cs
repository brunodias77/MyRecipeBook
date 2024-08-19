using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities;

public class DishType : Entity
{
    [Required] public int Type { get; set; }

    [Required] public Guid RecipeId { get; set; }

    [ForeignKey("RecipeId")] public Recipe Recipe { get; set; }
}