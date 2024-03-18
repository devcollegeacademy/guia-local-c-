using Microsoft.OpenApi.Models;
using guialocal.Data;
using guialocal.Services;
using guialocal.Middlewares;
using guialocal.Models;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

ConfigureCors(builder);

ConfigureDB(builder);

ConfigureSwagger(builder);

ConfigureScoped(builder);

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello, world!");

app.Urls.Add("http://*:5074");

var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
db.Database.Migrate();

app.UseCors("AllowAllOrigins");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Guia Local API v1");
});

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => { _ = endpoints.MapControllers(); });

app.Run();

void ConfigureDB(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
}

void ConfigureSwagger(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(options =>
    {

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Guia Local API",
            Version = "v1",
            Description = "A simple API for managing tasks",
            Contact = new OpenApiContact
            {
                Name = "Diego Pereira",
                Email = "dhiegopereira@devcollege.com.br",
                Url = new Uri("https://devcollegeacademy.com.br/")
            }
        });
    });
}

void ConfigureCors(WebApplicationBuilder builder)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    });
}

void ConfigureScoped(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<CustomerService>();

    builder.Services.AddScoped<IValidator<Customer>, CustomerCreateValidator>();
    builder.Services.AddScoped<IValidator<string?>, CustomerReadByFilterValidator>();
    builder.Services.AddScoped<IValidator<string>, CustomerReadOneValidator>();
    builder.Services.AddScoped<IValidator<(string, Customer)>, CustomerUpdateValidator>();
    builder.Services.AddScoped<IValidator<string>, CustomerDeleteValidator>();
}

