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
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;
using MassTransit;
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

builder.Services.AddHealthChecks();

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

// Register background services.
builder.Services.AddHostedService<HeartbeatBackgroundService>();

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

// ------------------------------------------------------------
// Static files
// ------------------------------------------------------------

var staticFileContentTypeProvider = new FileExtensionContentTypeProvider();

staticFileContentTypeProvider.Mappings[".wasm"] = "application/wasm";
staticFileContentTypeProvider.Mappings[".dat"] = "application/octet-stream";
staticFileContentTypeProvider.Mappings[".dll"] = "application/octet-stream";
staticFileContentTypeProvider.Mappings[".json"] = "application/json";
staticFileContentTypeProvider.Mappings[".br"] = "application/octet-stream";
staticFileContentTypeProvider.Mappings[".gz"] = "application/gzip";

// This serves Blazor WebAssembly framework files under /blazor.
// Example: /blazor/_framework/blazor.webassembly.js
app.UseBlazorFrameworkFiles("/blazor");

// This serves normal static files from API wwwroot and referenced static web assets.
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = staticFileContentTypeProvider,
    OnPrepareResponse = context =>
    {
        if (app.Environment.IsDevelopment())
        {
            context.Context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
            context.Context.Response.Headers.Pragma = "no-cache";
            context.Context.Response.Headers.Expires = "0";
        }
    }
});

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();

// ------------------------------------------------------------
// API / SPA routes
// ------------------------------------------------------------

app.MapGet("/admin", context =>
{
    context.Response.Redirect("/blazor/admin/dashboard");
    return Task.CompletedTask;
});

app.MapGet("/admin/{*path}", (string path, HttpContext context) =>
{
    context.Response.Redirect($"/blazor/admin/{path}");
    return Task.CompletedTask;
});

// Redirect /admin-login-bridge?token=xxx to /blazor/admin-login-bridge?token=xxx
app.MapGet("/admin-login-bridge", context =>
{
    var queryString = context.Request.QueryString.Value;

    var redirectUrl = string.IsNullOrWhiteSpace(queryString)
        ? "/blazor/admin-login-bridge"
        : $"/blazor/admin-login-bridge{queryString}";

    context.Response.Redirect(redirectUrl);
    return Task.CompletedTask;
});

// Debug endpoint.
app.MapGet("/debug-blazor", () =>
{
    return Results.Ok(new
    {
        Message = "API is running. Blazor static web assets should be served from /blazor.",
        TestBlazorRuntimeUrl = "/blazor/_framework/blazor.webassembly.js",
        TestBlazorIndexUrl = "/blazor/"
    });
});

// Default route opens Angular app.
app.MapGet("/", context =>
{
    context.Response.Redirect("/angular");
    return Task.CompletedTask;
});

// ------------------------------------------------------------
// SPA fallbacks
// ------------------------------------------------------------

// Blazor fallback.
// This sends Blazor index.html for /blazor routes like:
// /blazor/
// /blazor/admin/dashboard
// /blazor/admin-login-bridge
app.MapFallbackToFile("/blazor/{*path:nonfile}", "blazor/index.html");
// Angular fallback.
app.MapFallback(async context =>
{
    var requestPath = context.Request.Path.Value ?? string.Empty;

    if (IsSpaRoute(requestPath, "/angular"))
    {
        var webRootPath = app.Environment.WebRootPath
            ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");

        await SendSpaIndexAsync(
            context,
            webRootPath,
            "angular");

        return;
    }

    context.Response.StatusCode = StatusCodes.Status404NotFound;
});

await app.RunAsync();

static bool IsSpaRoute(string requestPath, string spaBasePath)
{
    if (requestPath.Equals(spaBasePath, StringComparison.OrdinalIgnoreCase))
    {
        return true;
    }

    if (requestPath.Equals($"{spaBasePath}/", StringComparison.OrdinalIgnoreCase))
    {
        return true;
    }

    if (requestPath.StartsWith($"{spaBasePath}/", StringComparison.OrdinalIgnoreCase)
        && !Path.HasExtension(requestPath))
    {
        return true;
    }

    return false;
}

static async Task SendSpaIndexAsync(
    HttpContext context,
    string webRootPath,
    string spaFolderName)
{
    var indexPath = Path.Combine(
        webRootPath,
        spaFolderName,
        "index.html");

    if (!System.IO.File.Exists(indexPath))
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;

        await context.Response.WriteAsync(
            $"{spaFolderName} index.html was not found at: {indexPath}");

        return;
    }

    context.Response.ContentType = "text/html";

    await context.Response.SendFileAsync(indexPath);
}