using IlnarApp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace IlnarApp.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
	: IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		
		builder.Entity<ApplicationUser>().ToTable("Users");
		builder.Entity<ApplicationRole>().ToTable("Roles");
		builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
		builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
		builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
		builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
		builder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken");

		builder.Entity<NoteType>();
		builder.Entity<Note>();
		builder.Entity<Tag>();
		builder.Entity<Archive>();
	}
}

public class ManagerDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var configuration = BuildConfiguration();
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseNpgsql(configuration.GetConnectionString("Default"));
		return new ApplicationDbContext(builder.Options);
	}
	private static IConfigurationRoot BuildConfiguration()
	{
		var builder = new ConfigurationBuilder()
			.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../IlnarApp.Api/"))
			.AddJsonFile("appsettings.json", optional: false);
		return builder.Build();
	}
}