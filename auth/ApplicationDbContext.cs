
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using juliWebApi.model;
using juliWebApi.entity;

namespace juliWebApi.auth
{
    public class ApplicationDbContext : IdentityDbContext<MyIdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TableRatings>()
                .HasMany(tr => tr.Ratings)
                .WithOne(r => r.TableRatings);
        }
        
        public DbSet<Table> Tables => Set<Table>();

        public DbSet<Rating> Ratings => Set<Rating>();

        public DbSet<TableRatings> TableRatings => Set<TableRatings>();

        public DbSet<TableUsers> TableUsers => Set<TableUsers>();

        public DbSet<Invitation> Invitations => Set<Invitation>();

        public DbSet<DefaultTable> DefaultTables => Set<DefaultTable>();
    }
}