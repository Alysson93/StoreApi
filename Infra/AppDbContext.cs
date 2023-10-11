using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
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

		builder.Ignore<Notification>();

		builder.Entity<Product>().Property(p => p.Name).IsRequired();
		builder.Entity<Product>().Property(p => p.Description).HasMaxLength(255);
		
		builder.Entity<Category>().Property(c => c.Name).IsRequired();
	
	}

}