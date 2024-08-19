using System.Diagnostics.CodeAnalysis;

namespace MRB.Domain.Extensions;

public static class StringExtension
{
    public static bool NotEmpty([NotNullWhen(true)] this string? value) => !string.IsNullOrWhiteSpace(value);
}