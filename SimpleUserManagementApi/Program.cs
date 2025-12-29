using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SimpleUserManagementApi.DataBase;
using SimpleUserManagementApi.Exceptions;
using Scalar.AspNetCore;
using SimpleUserManagementApi.Auth.JWT;
using SimpleUserManagementApi.PostManager.Interfaces;
using SimpleUserManagementApi.PostManager.Repositories;
using SimpleUserManagementApi.PostManager.Services;
using SimpleUserManagementApi.UserManager.Interfaces;
using SimpleUserManagementApi.UserManager.Repositories;
using SimpleUserManagementApi.UserManager.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Configuration.AddEnvironmentVariables();


builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {   
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
    });

builder.Services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

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
                ArgumentException => 400,
                _ => 500
            };
            context.Response.ContentType = "application/json";
            var response = new { error = exception?.Message };
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        });
    });


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();