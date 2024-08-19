using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities;

public class Ingredient : Entity
{


    [Required]
    [MaxLength(255)]
    public string Item { get; set; }

    [Required]
    public Guid RecipeId { get; set; }

    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }

}