using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MRB.CommonTest.BlobStorage;
using MRB.CommonTest.Entities;
using MRB.Domain.Entities;
using MRB.Domain.Enums;
using MRB.Domain.Security;
using MRB.Domain.Security.Token;
using MRB.Infra.Data;
using MRB.Infra.Security.Tokens.Generator;

namespace MRB.WebApiTest;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public CustomWebApplicationFactory()
    {
        (_user, _password) = UserBuilder.Build();
        _recipe = RecipeBuilder.Build(_user);
    }

    private User _user;
    private string _password;
    private Recipe _recipe;

    // Sobrescreve o método para configurar o host da aplicação web
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Define o ambiente como "Test" para a aplicação web
        builder.UseEnvironment("Test")
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Test.json", optional: false);
            })
            .ConfigureServices((context, services) =>
            {
                // Remove a configuração existente do DbContextOptions<ApplicationDbContext> se existir
                var descriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                // Configura um provedor de serviços para o banco de dados em memória
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                var blobStorage = new BlobStorageServiceBuilder().Build();
                services.AddScoped(option => blobStorage);

                // Configura o DbContext usando um banco de dados em memória para testes
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting"); // Nome do banco de dados em memória
                    options.UseInternalServiceProvider(provider); // Usa o provedor de serviços configurado
                });

                // Reconfigure the JwtTokenGenerator with test settings
                services.AddSingleton<IAccessTokenGenerator>(provider =>
                {
                    var configuration = context.Configuration;
                    var signingKey = configuration["Settings:Jwt:SigninKey"];
                    var expiresMinutesStr = configuration["Settings:Jwt:ExpirationTimeMinutes"];
                    if (string.IsNullOrEmpty(signingKey) || string.IsNullOrEmpty(expiresMinutesStr))
                    {
                        throw new InvalidOperationException("Jwt configuration is missing or invalid.");
                    }

                    var expiresMinutes = uint.Parse(expiresMinutesStr);
                    return new JwtTokenGenerator(expiresMinutes, signingKey);
                });

                // Cria um escopo de serviço para interagir com o DbContext
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();

                // Garante que o banco de dados em memória está vazio antes de iniciar
                dbContext.Database.EnsureDeleted();

                StartDataBase(dbContext, passwordEncripter);
            });
    }

    private void StartDataBase(ApplicationDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
        _user.Password = passwordEncripter.Encrypt(_user.Password);
        dbContext.Users.Add(_user);
        dbContext.Recipes.Add(_recipe);
        dbContext.SaveChanges();
    }

    public Guid GetRecipeId() => _recipe.Id;
    public string GetRecipeTitle() => _recipe.Title;
    public Difficulty GetRecipeDifficulty() => (Difficulty)_recipe.Difficulty!.Value;

    public CookingTime GetRecipeCookingTime() => (CookingTime)_recipe.CookingTime!.Value;
    public IList<MRB.Domain.Enums.DishType> GetRecipeDishTypes() => _recipe.DishTypes.Select(d => (MRB.Domain.Enums.DishType)d.Type).ToList();

    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetUserPasword() => _user.Password;
    public Guid GetUserId() => _user.Id;
}