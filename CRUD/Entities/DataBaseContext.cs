using Microsoft.EntityFrameworkCore;

namespace CRUD.Entities
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
	}
}
