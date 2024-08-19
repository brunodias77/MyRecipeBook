using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MRB.Api.Binders;

public class MyRecipeBookRecipeIdBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
        var value = valueProviderResult.FirstValue;
        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        // Aqui seria pra tranformar o id (string) da requisicao em id (long) que precisamos para o useCase.
        var id = value;
        bindingContext.Result = ModelBindingResult.Success(id);
        return Task.CompletedTask;
    }
}