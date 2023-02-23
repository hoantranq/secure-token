using Microsoft.EntityFrameworkCore;
using Secure.API.Models;

namespace Secure.API.Contexts;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}

	public DbSet<CertificateCategory> CertificateCategories { get; set; }
}