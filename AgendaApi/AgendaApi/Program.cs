using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Application.Contacts.Validators;
using AgendaApi.Domain.Interfaces;
using AgendaApi.Infra.Data;
using AgendaApi.Infra.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsForAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(typeof(AgendaApi.Application.Mapping.MappingProfile));

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateContactCommand).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<CreateContactValidator>();

builder.Services.AddTransient<IValidator<CreateContactCommand>, CreateContactValidator>();

builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.AddScoped<IValidator<UpdateContactCommand>, UpdateContactValidator>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Agenda API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agenda API V1"));

app.UseCors("CorsForAll");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();