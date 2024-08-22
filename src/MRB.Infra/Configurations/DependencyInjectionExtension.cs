using System.Reflection;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRB.Domain.Extensions;
using MRB.Domain.Repositories;
using MRB.Domain.Security;
using MRB.Domain.Security.Token;
using MRB.Domain.Services;
using MRB.Domain.Services.OpenAI;
using MRB.Domain.Services.ServiceBus;
using MRB.Domain.Services.Storage;
using MRB.Infra.Data;
using MRB.Infra.Data.Repositories;
using MRB.Infra.Security.Tokens.Generator;
using MRB.Infra.Security.Tokens.Validator;
using MRB.Infra.Services.LoggedUsers;
using MRB.Infra.Services.OpenAI;
using MRB.Infra.Services.ServiceBus;
using MRB.Infra.Services.Storage;
using OpenAI_API;

namespace MRB.Infra.Configurations;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        AddQueue(services, configuration);
        AddPasswordEncrypter(services, configuration);
        AddDbContext(services, configuration);
        AddLoggedUser(services);
        AddToken(services, configuration);
        AddRepositories(services);
        AddOpenAI(services, configuration);
        AddFluenteMigrator(services, configuration);
        AddAzureStorage(services, configuration);
    }

    private static void AddDbContext(IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var version = new Version(8, 4, 0);
        var serverVersion = new MySqlServerVersion(version);

        services.AddDbContext<ApplicationDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }

    private static void AddToken(IServiceCollection services, IConfigurationManager configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigninKey");

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddFluenteMigrator(IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options.AddMySql8().WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MRB.Infra")).For.All();
        });
    }

    private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter, Security.BCrypt>();
    }

    private static void AddLoggedUser(IServiceCollection services)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddOpenAI(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGenerateRecipeAI, ChatGptService>();
        var key = configuration.GetValue<string>("Settings:OpenAI:ApiKey");
        var authentication = new APIAuthentication(key);
        services.AddScoped<IOpenAIAPI>(options => new OpenAIAPI(authentication));
    }

    private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

        if (connectionString != null)
        {
            services.AddScoped<IBlobStorageService>(c =>
                new AzureStorageService(new BlobServiceClient(connectionString)));
        }
    }

    private static void AddQueue(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount")!;

        if (string.IsNullOrWhiteSpace(connectionString))
            return;

        var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        });

        var deleteQueue = new DeleteUserQueue(client.CreateSender("user"));

        var deleteUserProcessor = new DeleteUserProcessor(client.CreateProcessor("user", new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1
        }));

        services.AddSingleton(deleteUserProcessor);

        services.AddScoped<IDeleteUserQueue>(options => deleteQueue);
    }
}