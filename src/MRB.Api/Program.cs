using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using MRB.Api.BackgroundServices;
using MRB.Api.Converters;
using MRB.Api.Filters;
using MRB.Api.Middlewares;
using MRB.Api.Tokens;
using MRB.Application.Configurations;
using MRB.Domain.Security.Token;
using MRB.Infra.Configurations;
using MRB.Infra.Extensions;
using MRB.Infra.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<IdsFilter>();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                    Enter 'Bearer' [space] and then your token in the text input below.
                    Example: 'Bearer 1234abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    options.CustomSchemaIds(type =>
    {
        if (type == typeof(MRB.Domain.Entities.DishType))
            return "DishType";
        if (type == typeof(MRB.Domain.Enums.DishType))
            return "DishTypeEnum";
        return type.FullName; // Usa o nome completo do tipo como identificador de esquema por padrão
    });
});
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<DeleteUserService>();
AddGoogleAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

await app.RunAsync();


void MigrateDatabase()
{
    if (builder.Configuration.IsUnitTestEnvironment())
        return;

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    DataBaseMigration.Migrate(builder.Configuration.ConnectionString(), serviceScope.ServiceProvider);
}

void AddGoogleAuthentication()
{
    var clientId = builder.Configuration.GetValue<string>("Settings:Google:ClientId")!;
    var clientSecret = builder.Configuration.GetValue<string>("Settings:Google:ClientSecret")!;

    builder.Services.AddAuthentication(config =>
        {
            config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie()
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = clientId;
            googleOptions.ClientSecret = clientSecret;
        });
}

public partial class Program
{
    protected Program()
    {
    }
}