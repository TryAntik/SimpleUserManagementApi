using System.Text.Json;
using FluentValidation;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using SimpleUserManagementApi.Auth.Extensions;
using SimpleUserManagementApi.Auth.RefreshToken;
using SimpleUserManagementApi.DataBase;
using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.PostManager.Services;
using SimpleUserManagementApi.UserManager.Services;
using SimpleUserManagementApi.UserManager.Interfaces;
using SimpleUserManagementApi.PostManager.Interfaces;
using SimpleUserManagementApi.PostManager.Repositories;
using SimpleUserManagementApi.UserManager.Repositories;
using SimpleUserManagementApi.UserManager.Validators;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {   
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.Configure<RefreshTokenSettings>(configuration.GetSection("RefeshTokenSettings"));

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserDTOValidator>();

builder.Services.AddAuth(configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi(); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar"));
}
 

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        context.Response.StatusCode = exception switch
        {
            NotFoundException => 404,
            BadRequestException => 400,
            ValidationException => 400,
            _ => 500
        };
        context.Response.ContentType = "application/json";
        var response = new
        {
            error = exception?.Message, 
            statusCode = context.Response.StatusCode
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();