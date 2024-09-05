using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities
{
    public class RefreshToken : Entity
    {
        public required string Value { get; set; } = string.Empty;
        public required long UserId { get; set; }
        public User User { get; set; } = default!;
    }
}