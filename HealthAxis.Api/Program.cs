using HealthAxis.Api.BackgroundServices;
using HealthAxis.Api.Data;
using HealthAxis.Api.Mapping;
using HealthAxis.Api.Middleware;
using HealthAxis.Api.Repository.Impl;
using HealthAxis.Api.Repository.Interface;
using HealthAxis.Api.Services;
using HealthAxis.Api.Services.Impl;
using HealthAxis.Api.Services.Interface;
using HealthCareApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;
using MassTransit;
using HealthAxis.Api.Options;
using HealthAxis.Api.Messaging.Consumers;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information("HealthAxis App Api Starting ...");


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog.
builder.Services.AddSerilog((services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// Register garnet hosted service for background processing.
builder.Services.AddHostedService<GarnetHostedService>();

// Register Garnet options from configuration.
builder.Services.Configure<GarnetOptions>(
    builder.Configuration.GetSection("Garnet"));

// Register distributed cache using embedded Garnet.
builder.Services.AddStackExchangeRedisCache(options =>
{
    var garnetOptions = builder.Configuration
        .GetSection("Garnet")
        .Get<GarnetOptions>()!;

    options.Configuration = garnetOptions.ConnectionString;
    options.InstanceName = garnetOptions.InstanceName;
});

// Register HealthAxisDbContext with SQL Server.
builder.Services.AddDbContext<HealthAxisDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbCon")));

// Register ASP.NET Core Identity.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<HealthAxisDbContext>()
.AddDefaultTokenProviders();

// Register JWT Authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwt["Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HealthApp API",
        Version = "v1"
    });

    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter JWT token only. Do not type Bearer."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});


// Register DbContext for generic repository constructor.
builder.Services.AddScoped<DbContext, HealthAxisDbContext>();

// Register AuthService.
builder.Services.AddScoped<IAuthService, AuthService>();

// Register AutoMapper.
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// Register generic repository.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register entity-specific repositories.
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();


// Register services.
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
builder.Services.AddScoped<ICacheService, CacheService>();


// Register background services.
builder.Services.AddHostedService<HeartbeatBackgroundService>();
//builder.Services.AddHostedService<NotificationCleanupService>();

// Register MassTransit with RabbitMQ.
var rabbitmqConfig = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AppointmentBookedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitmqConfig["HostName"], rabbitmqConfig["VirtualHost"], h =>
        {
            h.Username(rabbitmqConfig["Username"]!);
            h.Password(rabbitmqConfig["Password"]!);
        });
        cfg.ReceiveEndpoint("appointment-booked-queue", e =>
        {
            e.ConfigureConsumer<AppointmentBookedConsumer>(context);
        });

    });
});

// Register Global Exception Handler.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


const string ClientCorsPolicy = "ClientCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(ClientCorsPolicy, policy =>
    {
        policy.WithOrigins(
                "https://localhost:7075",
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSerilogRequestLogging();

// Seed roles and default admin.
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    await RoleSeeder.SeedRoleAsync(roleManager);

    await AdminSeeder.SeedAdminAsync(userManager, roleManager, builder.Configuration);
}

// Global exception handler middleware.
app.UseExceptionHandler();

// Configure HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors(ClientCorsPolicy);
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


await app.RunAsync();
