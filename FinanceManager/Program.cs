using FinanceManager.Dtos.Auth;
using FinanceManager.Middleware;
using FinanceManager.Models;
using FinanceManager.Repository.Auth;
using FinanceManager.Services;
using FinanceManager.Services.Auth;
using FinanceManager.Validators.Auth;
using FluentValidation;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Auth
builder.Services.AddScoped<IAuthRepository<User>, AuthRepository>();
builder.Services.AddScoped<IAuthService<UserDto, AuthRegisterDto, AuthLoginDto>, AuthService>();

// Jwt
builder.Services.AddSingleton<JwtService>();
builder.Services.AddHttpContextAccessor();

// Entity Framework
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"));
});

// Validators
builder.Services.AddScoped<IValidator<AuthRegisterDto>, AuthRegisterValidator>();
builder.Services.AddScoped<IValidator<AuthLoginDto>, AuthLoginValidation>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always
});

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api/auth/login") && !context.Request.Path.StartsWithSegments("/api/auth/login"), configuration =>
{
    configuration.UseMiddleware<VerifyToken>();
});

app.Run();
