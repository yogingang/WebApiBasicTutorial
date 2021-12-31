using Microsoft.EntityFrameworkCore;
using WebApiBasicTutorial.Infrastructure.Models;

namespace WebApiBasicTutorial.Infrastructure
{
    public class MyDbContext:DbContext
    {
        public MyDbContext()
        {

        }

        protected MyDbContext(DbContextOptions options) : base(options)
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<User>(entity =>
            {
                // Table 이름은 User 로 한다. 
                entity.ToTable("User");

                // Primary key 는 Id 이다.
                entity.HasKey(e => e.Id);

                // Id 는 필수 값이다.
                // Id 는 varchar 형태로 20 length 이다. 
                entity.Property(e => e.Id)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(20);

                // Name 은 256 length 를 갖는 nvarchar 형태이다.
                entity.Property(e => e.Name)
                    .HasMaxLength(256);

                // Email 은 256 length 를 갖는 nvarchar 형태이다.
                entity.Property(e => e.Email)
                    .HasMaxLength(256);

            });
        }
    }
}
