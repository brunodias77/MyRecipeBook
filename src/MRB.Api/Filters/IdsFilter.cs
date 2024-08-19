using Microsoft.OpenApi.Models;
using MRB.Api.Binders;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MRB.Api.Filters
{
    // Classe que implementa a interface IOperationFilter para modificar a documentação do Swagger
    public class IdsFilter : IOperationFilter
    {
        // Método Apply é chamado para modificar a operação do Swagger
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Obter todos os parâmetros que usam MyRecipeBookRecipeIdBinder
            var encryptedIds = context.ApiDescription.ParameterDescriptions
                .Where(x => x.ModelMetadata.BinderType == typeof(MyRecipeBookRecipeIdBinder))
                .ToDictionary(d => d.Name, d => d);

            // Modificar os parâmetros na operação do Swagger
            foreach (var parameter in operation.Parameters)
            {
                // Verificar se o parâmetro está na lista de parâmetros encriptados
                if (encryptedIds.TryGetValue(parameter.Name, out var apiParameter))
                {
                    // Definir o formato e tipo do parâmetro como string
                    parameter.Schema.Format = string.Empty;
                    parameter.Schema.Type = "string";
                }
            }

            // Modificar os esquemas de propriedades na documentação do Swagger
            foreach (var schema in context.SchemaRepository.Schemas.Values)
            {
                foreach (var property in schema.Properties)
                {
                    // Verificar se a propriedade está na lista de parâmetros encriptados
                    if (encryptedIds.TryGetValue(property.Key, out var apiParameter))
                    {
                        // Definir o formato e tipo da propriedade como string
                        property.Value.Format = string.Empty;
                        property.Value.Type = "string";
                    }
                }
            }
        }
    }
}