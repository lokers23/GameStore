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
using System.Security.Claims;
using System.Text.Json.Serialization;
using GameStore.Service;
using GameStore.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    });


builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddEndpointsApiExplorer();
#region Services for add to scope service
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDeveloperService, DeveloperService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IMinSpecificationService, MinSpecificationService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<IActivationService, ActivationService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IKeyService, KeyService>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
#endregion

#region Services for add to scope repository
builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Developer>, DeveloperRepository>();
builder.Services.AddScoped<IRepository<Publisher>, PublisherRepository>();
builder.Services.AddScoped<IRepository<MinimumSpecification>, MinSpecificationRepository>();
builder.Services.AddScoped<IRepository<Image>, ImageRepository>();
builder.Services.AddScoped<IRepository<Platform>, PlatformRepository>();
builder.Services.AddScoped<IRepository<Activation>, ActivationRepository>();
builder.Services.AddScoped<IRepository<Game>, GameRepository>();
builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
builder.Services.AddScoped<IRepository<Key>, KeyRepository>();
#endregion

builder.Services.AddDbContext<GamestoredbContext>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "GameStoreAPI", Version = "v1" } );
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Bearer",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
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
    config.AddPolicy(nameof(AccessRole.Administrator), policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, nameof(AccessRole.Administrator));
    });

    config.AddPolicy(nameof(AccessRole.Moderator), policy =>
    {
        policy.RequireAssertion(x => 
                                      x.User.HasClaim(ClaimTypes.Role, nameof(AccessRole.Administrator))
                                   || x.User.HasClaim(ClaimTypes.Role, nameof(AccessRole.Moderator)));
    });

    config.AddPolicy(nameof(AccessRole.User), policy =>
    {
        policy.RequireAssertion(x => 
                                      x.User.HasClaim(ClaimTypes.Role, nameof(AccessRole.Administrator))
                                   || x.User.HasClaim(ClaimTypes.Role, nameof(AccessRole.Moderator))
                                   || x.User.HasClaim(ClaimTypes.Role, nameof(AccessRole.User)));
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<GamestoredbContext>();
    dataContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GameStoreAPI");
    });
}

app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();