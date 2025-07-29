using IlnarApp.Application.Repositories;
using IlnarApp.Domain.Archive;
using IlnarApp.Domain.Note;
using IlnarApp.Domain.Tag;
using IlnarApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
	options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSerilog(lc => lc
	.WriteTo.Console()
	.WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "Logs/log-.log"), 
		LogEventLevel.Error, rollingInterval: RollingInterval.Day));

builder.Services.AddControllers();

builder.Services.AddScoped<INoteTypeRepository, NoteTypeRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IArchiveRepository, ArchiveRepository>();

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

app.MapControllers();

app.Run();
