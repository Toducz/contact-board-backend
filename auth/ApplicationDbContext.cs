
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using juliWebApi.model;
using juliWebApi.entity;

namespace juliWebApi.auth
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Table>()
                .HasMany(t => t.SharedWith)
                .WithMany(u => u.SharedWith);
                //.OnDelete(DeleteBehavior.Cascade);
    

            builder.Entity<Table>()
                .HasMany(t => t.Ratings)
                .WithOne(u => u.Table);
                //.OnDelete(DeleteBehavior.Cascade);
                

            builder.Entity<User>()
                .HasMany<Table>()
                .WithOne(t => t.UserOwner);
                
            builder.Entity<Table>()
                .HasMany<User>()
                .WithOne(u => u.Table);

            builder.Entity<User>()
                .HasMany<Rating>()
                .WithOne( r => r.User)
                .HasForeignKey( r => r.UserForeignKey);

            builder.Entity<User>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<User>(u => u.UserId );

        }

        public DbSet<Table> Tables => Set<Table>();

        public DbSet<User> Users => Set<User>();

        //public DbSet<Rating> Ratings => Set<Rating>();
    }
}