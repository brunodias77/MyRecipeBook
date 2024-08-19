using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities;

public class User : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}