using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<IdentityUser>
{


	public DbSet<Product> Products { get; set; }
	public DbSet<Category> Categories { get; set; }

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

	protected override void ConfigureConventions(ModelConfigurationBuilder config)
	{
		config.Properties<string>().HaveMaxLength(100);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{

		base.OnModelCreating(builder);

		builder.Ignore<Notification>();

		builder.Entity<Product>().Property(p => p.Name).IsRequired();
		builder.Entity<Product>().Property(p => p.Description).HasMaxLength(255);
		
		builder.Entity<Category>().Property(c => c.Name).IsRequired();
	
	}

}