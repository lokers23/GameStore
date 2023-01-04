using GameStore.DAL;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Repositories;
using GameStore.Domain.Enums;
using GameStore.Domain.Models;
using GameStore.Service.Interfaces;
using GameStore.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; 
    });


builder.Logging.ClearProviders();
builder.Host.UseNLog();

#region Services for add to scope
builder.Services.AddScoped<IGenreService, GenreService>();
//builder.Services.AddScoped<IGameService, GameService>();
//builder.Services.AddScoped<IActivationService, ActivationService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
//builder.Services.AddScoped<IRepository<Game>, GameRepository>();
//builder.Services.AddScoped<IRepository<Activation>, ActivationRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();

builder.Services.AddDbContext<GamestoredbContext>();
#endregion

builder.Services.AddCors();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(config =>
    {
        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.
                GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(nameof(AccessRole.Administrator), builder =>
    {
        builder.RequireClaim(ClaimTypes.Role, nameof(AccessRole.Administrator));
    });

    config.AddPolicy(nameof(AccessRole.Moderator), builder =>
    {
        builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, nameof(AccessRole.Administrator))
                                   || x.User.HasClaim(ClaimTypes.Role, nameof(AccessRole.Moderator)));
    });

    config.AddPolicy(nameof(AccessRole.User), builder =>
    {
        builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, nameof(AccessRole.Administrator))
                                   || x.User.HasClaim(ClaimTypes.Role, nameof(AccessRole.Moderator))
                                   || x.User.HasClaim(ClaimTypes.Role, nameof(AccessRole.User)));
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "OAuth2",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();


app.UseCors(c => c.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<GamestoredbContext>();
    dataContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();