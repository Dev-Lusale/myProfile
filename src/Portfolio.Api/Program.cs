using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Api.Data;
using Portfolio.Api.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<PortfolioDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Use in-memory database for development/testing
        options.UseInMemoryDatabase("PortfolioDb");
    }
    else
    {
        // Use PostgreSQL for production (Railway/Heroku) or SQL Server (Azure)
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        
        if (connectionString?.Contains("postgres") == true)
        {
            options.UseNpgsql(connectionString);
        }
        else
        {
            options.UseSqlServer(connectionString);
        }
    }
});

// Add Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<PortfolioDbContext>()
.AddDefaultTokenProviders();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "PortfolioApi",
        ValidAudience = jwtSettings["Audience"] ?? "PortfolioClient",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Add services
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// Configure Email Settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins("https://localhost:7001", "http://localhost:5001", "http://localhost:5000", "http://localhost:5002")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
        else
        {
            // Production domains - these will be configured via environment variables or appsettings
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new[]
            {
                "https://bernard-portfolio-web.azurewebsites.net",
                "https://www.bernard-lusale.com",
                "https://bernard-lusale.com",
                "https://bernard-portfolio-api.azurewebsites.net"
            };
            
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorWasm");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created and migrated
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
    
    if (app.Environment.IsDevelopment())
    {
        context.Database.EnsureCreated();
    }
    else
    {
        // Use migrations in production
        context.Database.Migrate();
    }
}

app.Run();