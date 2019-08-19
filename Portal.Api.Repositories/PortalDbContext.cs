using Assette.Client;
using Microsoft.EntityFrameworkCore;

namespace Portal.Api.Repositories
{
    public class PortalDbContext:DbContext
    {
       
        public DbSet<UserDto> Users { get; set; }
        public DbSet<DocumentDto> Documents { get; set; }
        protected string _dbName { get; private set; }
       
        public PortalDbContext(DbContextOptions<PortalDbContext> options):base (options)
        {

        }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite($"FileName={_dbName}", options=> {
            //    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            //});
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<UserDto>(entity=> {
            //    entity.HasKey(e => e.UserId);
            //});
            //base.OnModelCreating(modelBuilder);

        }
    }
}
