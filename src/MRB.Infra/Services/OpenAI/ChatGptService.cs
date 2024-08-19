using MRB.Domain.Enums;
using MRB.Domain.Services.OpenAI;
using OpenAI_API;
using OpenAI_API.Chat;

namespace MRB.Infra.Services.OpenAI;

public class ChatGptService : IGenerateRecipeAI
{
    private const string CHAT_MODEL = "gpt-3.5-turbo";
    private readonly IOpenAIAPI _openAiapi;

    public ChatGptService(IOpenAIAPI openAiapi)
    {
        _openAiapi = openAiapi;
    }

    public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        var conversation = _openAiapi.Chat.CreateConversation(new ChatRequest { Model = CHAT_MODEL });
        conversation.AppendSystemMessage(ResourceOpenAI.STARTING_GENERATE_RECIPE);
        conversation.AppendUserInput(string.Join(";", ingredients));
        var response = await conversation.GetResponseFromChatbotAsync();
        var responseList = response.Split("\n")
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Select(item => item.Replace("[", "").Replace("]", ""))
            .ToList();

        var step = 1;
        return new GeneratedRecipeDto
        {
            Title = responseList[0],
            CookingTime = (CookingTime)Enum.Parse(typeof(CookingTime), responseList[1]),
            Ingredients = responseList[2].Split(";"),
            Instructions = responseList[3].Split("@").Select(ins => new GeneratedInstructionDto
            {
                Text = ins.Trim(),
                Step = step++
            }).ToList()
        };
    }
}