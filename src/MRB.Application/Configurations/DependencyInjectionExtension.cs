using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRB.Application.Services;
using MRB.Application.UseCases.Dashboard.GetRecipes;
using MRB.Application.UseCases.Recipes.Delete;
using MRB.Application.UseCases.Recipes.Filter;
using MRB.Application.UseCases.Recipes.GenerateChatGpt;
using MRB.Application.UseCases.Recipes.GetById;
using MRB.Application.UseCases.Recipes.Image;
using MRB.Application.UseCases.Recipes.Register;
using MRB.Application.UseCases.Recipes.Update;
using MRB.Application.UseCases.Users.Delete;
using MRB.Application.UseCases.Users.Delete.Delete;
using MRB.Application.UseCases.Users.Login;
using MRB.Application.UseCases.Users.Login.External;
using MRB.Application.UseCases.Users.Login.Google;
using MRB.Application.UseCases.Users.Profile;
using MRB.Application.UseCases.Users.Register;
using MRB.Application.UseCases.Users.Update;
using MyRecipeBook.Application.UseCases.Recipe.Image;
using Sqids;

namespace MRB.Application.Configurations;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services, configuration);
        AddPasswordEncrypter(services, configuration);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 10,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });

        // Se for usar o o sqids com autoMapper
        // services.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOptions =>
        // {
        //     sqids = option.GetService<SqidsEncoder<long>>();
        //     autoMapperOptions.AddProfile(new AutoMapping(sqids));
        // }));

        services.AddScoped(option =>
            new AutoMapper.MapperConfiguration(opt => { opt.AddProfile(new AutoMapping(sqids)); }).CreateMapper());
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
        services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
        services.AddScoped<IGetRecipeByIdUseCase, GetRecipeByIdUseCase>();
        services.AddScoped<IDeleteRecipeUseCase, DeleteRecipeUseCase>();
        services.AddScoped<IUpdateRecipeUseCase, UpdateRecipeUseCase>();
        services.AddScoped<IGetDashboardUseCase, GetDashboardUseCase>();
        services.AddScoped<IGenerateRecipeUseCase, GenerateRecipeUseCase>();
        services.AddScoped<IAddUpdateImageCoverUseCase, AddUpdateImageCoverUseCase>();
        services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
        services.AddScoped<IRequestDeleteUserUseCase, RequestDeleteUserUseCase>();
        services.AddScoped<IExternalLoginUseCase, LoginGoogleUseCase>();

    }

    private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
    {
        //var additionalKey = configuration.GetSection("Settings:Password:AdditionalKey").Value;
        // var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        // services.AddScoped(options => new PasswordEncripter(additionalKey));
    }

    private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 10,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });
        services.AddSingleton(sqids);
    }
}