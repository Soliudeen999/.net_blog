using Blog;
using Blog.App;
using Blog.Configs;
using Blog.Routes;
using Blog.Utils.Middlewares;
using Blog.Utils;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.RegisterCustomServices();
builder.RegisterServices();

var connString = builder.Configuration.GetConnectionString("AppDbConnectionString");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connString, ServerVersion.AutoDetect(connString)));


var app = builder.Build();

// Seed database with initial data
DatabaseSeeder.SeedDatabase(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseSession();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<SessionAuthMiddleware>();

app.ApplyCors();

app.UseHttpsRedirection();


app.AddRoutes();

app.Run();
