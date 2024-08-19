using Moq;
using MRB.Domain.Enums;
using MRB.Domain.Services.OpenAI;

namespace MRB.CommonTest.OpenAI;

public class GenerateRecipeAIBuilder
{
    public static IGenerateRecipeAI Build(GeneratedRecipeDto dto)
    {
        var mock = new Mock<IGenerateRecipeAI>();

        mock.Setup(service => service.Generate(It.IsAny<List<string>>())).ReturnsAsync(dto);

        return mock.Object;
    }
}