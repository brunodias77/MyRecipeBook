using Microsoft.AspNetCore.Http;

namespace MRB.Communication.Requests.Recipes.Register;

public class RequestRegisterRecipeFormData : RequestRegisterRecipeJson
{
    public IFormFile? Image { get; set; }
    
}