using IlnarApp.Api.Middleware;
using IlnarApp.Application.Options;
using IlnarApp.Application.Repositories;
using IlnarApp.Application.Services.Jwt;
using IlnarApp.Domain.Archive;
using IlnarApp.Domain.Identity;
using IlnarApp.Domain.Note;
using IlnarApp.Domain.Tag;
using IlnarApp.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(
	options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddSerilog(lc => lc
	.WriteTo.Console()
	.WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "Logs/log-.log"), 
		LogEventLevel.Error, rollingInterval: RollingInterval.Day));


builder.Services.AddControllers().AddNewtonsoftJson(options =>
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddAntiforgery();

builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth"));

var authOptions = builder.Configuration.GetSection("Auth").Get<AuthOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		// true in production
		options.RequireHttpsMetadata = false;

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = authOptions?.Issuer,
			ValidateAudience = true,
			ValidAudience = authOptions?.Audience,
			ValidateLifetime = true,
			IssuerSigningKey = authOptions?.GetSymmetricSecurityKey(),
			ValidateIssuerSigningKey = true
		};
	});

builder.Services
	.AddIdentityCore<ApplicationUser>()
	.AddRoles<ApplicationRole>()
	.AddUserManager<UserManager<ApplicationUser>>()
	.AddRoleManager<RoleManager<ApplicationRole>>()
	.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
	.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
	.AddDefaultTokenProviders();

builder.Services.AddScoped<INoteTypeRepository, NoteTypeRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IArchiveRepository, ArchiveRepository>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAntiforgery();

app.UseMiddleware<ApiExceptionsMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
