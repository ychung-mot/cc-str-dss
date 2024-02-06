using AdvSol.Authentication;
using AdvSol.Data;
using AdvSol.Data.Mappings;
using AdvSol.Data.Repositories;
using AdvSol.Services;
using AdvSol.Services.Dtos;
using AdvSol.Services.Jobs;
using Asp.Versioning;
using AutoMapper;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetCore.AutoRegisterDi;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//var connString = builder.Configuration.GetConnectionString("AdvSol");
var dbHost = builder.Configuration.GetValue<string>("DB_HOST");
var dbName = builder.Configuration.GetValue<string>("DB_NAME");
var dbUser = builder.Configuration.GetValue<string>("DB_USER");
var dbPass = builder.Configuration.GetValue<string>("postgres-password");

var connString = $"Host={dbHost};Username={dbUser};Password={dbPass};Database={dbName};Port=5432;";

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddDbContext<AppDbContext>(opt =>
//    opt.UseNpgsql(connString, opt => opt.UseNetTopologySuite()));

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(connString));

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new HeaderApiVersionReader("version");
    options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
});

var assemblies = Assembly.GetExecutingAssembly()
    .GetReferencedAssemblies()
    .Where(a => a.FullName.StartsWith("AdvSol"))
    .Select(Assembly.Load).ToArray();

//Services
builder.Services.RegisterAssemblyPublicNonGenericClasses(assemblies)
     .Where(c => c.Name.EndsWith("Service"))
     .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

//Repository
builder.Services.RegisterAssemblyPublicNonGenericClasses(assemblies)
     .Where(c => c.Name.EndsWith("Repository"))
     .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

//Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Headers
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

//FieldValidationService as Singleton
builder.Services.AddSingleton<IFieldValidatorService, FieldValidatorService>();

//RegexDefs as Singleton
builder.Services.AddSingleton<RegexDefs>();

builder.Services.AddHttpClient();

var mappingConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new EntityToModelProfile());
    cfg.AddProfile(new ModelToEntityProfile());
    cfg.AddProfile(new ModelToModelProfile());
});

var mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services
    .AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseMemoryStorage()
        //.UsePostgreSqlStorage((option) => 
        //    { 
        //        option.UseNpgsqlConnection(connString); 
        //    }
        //)
    ); ;

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 5;
});

builder.Services.AddScoped<AdvJwtBearerEvents>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenSecret = builder.Configuration.GetValue<string>("InternalTokenSecret") ?? "";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = TokenHelper.Issuer,
            ValidAudience = TokenHelper.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret)),
            LifetimeValidator = TokenLifetimeValidator.Validate,
        };

        options.EventsType = typeof(AdvJwtBearerEvents);
    })
;

builder.Services.AddAuthorization(options =>
{
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
        JwtBearerDefaults.AuthenticationScheme);
    defaultAuthorizationPolicyBuilder =
        defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var AllowedOrigins = "__AllowedOrigins__";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedOrigins,
        policy =>
        {
            policy
                .WithOrigins()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Content-Disposition")
                .AllowCredentials();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dataContext.Database.Migrate();
    SeedData.Initialize(dataContext);

    var commonCodeRepository = scope.ServiceProvider.GetRequiredService<ICommonCodeRepository>();
    var commonCodes = commonCodeRepository.GetCommonCodes();

    var fieldValidatorService = scope.ServiceProvider.GetRequiredService<IFieldValidatorService>();
    fieldValidatorService.CommonCodes = commonCodes;
}

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(AllowedOrigins);
}

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<HelloWorldJob>("Hello", job => job.ExecuteHelloWorld(), Cron.Minutely);

app.Run();
