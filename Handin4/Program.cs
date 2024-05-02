using Handin4.Data;
using Handin4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Emit;
using System.Security.Claims;
using Serilog;
using System.Text.Json.Serialization;
using Handin4.Services;
using Handin4.Services.Interfaces;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(cfg =>
//    {
//        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]);
//        cfg.AllowAnyHeader();
//        cfg.AllowAnyMethod();
//    });
//    options.AddPolicy(name: "AnyOrigin",
//        cfg =>
//        {
//            cfg.AllowAnyOrigin();
//            cfg.AllowAnyHeader();
//            cfg.AllowAnyMethod();
//        });
//});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter token"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDbContext<myDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
}).AddEntityFrameworkStores<myDbContext>();

// Add JWT authentication

builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.ReadFrom.Configuration(ctx.Configuration);
});
    

addJWTAuthentication(builder);

ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
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

void addJWTAuthentication(WebApplicationBuilder builder)
{
    var jwtKeySecret = builder.Configuration["JWT:SigningKey"];
    var jwtIssuerSecret = builder.Configuration["JWT:Issuer"];
    var jwtAudienceSecret = builder.Configuration["JWT:Audience"];

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                    options.DefaultScheme =
                        options.DefaultSignInScheme =
                            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuerSecret,
            ValidAudience = jwtAudienceSecret,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(
                    jwtKeySecret))
        };
        options.TokenValidationParameters.RoleClaimType = "Role";
        //options.TokenValidationParameters.NameClaimType = "Name";

        options.MapInboundClaims = false;
    });

    // Add authorization policies
    builder.Services.AddAuthorization(options =>
    {

        //options.AddPolicy("AdminAccess", policy =>
        //    policy.RequireAssertion(context =>
        //        context.User.IsInRole("Admin")));

        //options.AddPolicy("IngredientsAccess", policy =>
        //    policy.RequireAssertion(context =>
        //        context.User.IsInRole("Admin") ||
        //        context.User.IsInRole("Manager")));

        //options.AddPolicy("OrderAccess", policy =>
        //    policy.RequireAssertion(context =>
        //        context.User.IsInRole("Admin") ||
        //        context.User.IsInRole("Manager") ||
        //        context.User.IsInRole("Driver")));

        //options.AddPolicy("BakedGoodsAccess", policy =>
        //    policy.RequireAssertion(context =>
        //        context.User.IsInRole("Admin") ||
        //        context.User.IsInRole("Manager") ||
        //        context.User.IsInRole("Baker")));

        options.AddPolicy("AdminAccess", policy =>
            policy.RequireClaim(ClaimTypes.Role, "Admin"));

        options.AddPolicy("ManagerOrHigher", policy =>
            policy.RequireClaim(ClaimTypes.Role, "Manager", "Admin"));

        options.AddPolicy("BakerOrHigher", policy =>
            policy.RequireClaim(ClaimTypes.Role, "Baker", "Manager", "Admin"));

        options.AddPolicy("DriverOrHigher", policy =>
            policy.RequireClaim(ClaimTypes.Role, "Driver", "Manager", "Admin"));

    });
}


void ConfigureServices(IServiceCollection services)
{

    builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
    {
        var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
        return new MongoClient(mongoConnectionString); ;
    });

    services.AddScoped<ILogService, LogService>(serviceProvider =>
    {
        var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
        return new LogService(mongoClient);
    });
}