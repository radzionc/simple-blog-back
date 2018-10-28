using System.Linq;
using Blog.Model;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class BlogContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            ConfigureModelBuilderForUser(modelBuilder);
            ConfigureModelBuilderForStory(modelBuilder);
            ConfigureModelBuilderForLike(modelBuilder);
        }

        void ConfigureModelBuilderForUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .HasMaxLength(60)
                .IsRequired();
        }

        void ConfigureModelBuilderForStory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Story>().ToTable("Story");
            modelBuilder.Entity<Story>()
                .Property(s => s.Title)
                .HasMaxLength(100);
            
            modelBuilder.Entity<Story>()
                .Property(s => s.OwnerId)
                .IsRequired();
            
            modelBuilder.Entity<Story>()
                .HasOne(s => s.Owner)
                .WithMany(u => u.Stories)
                .HasForeignKey(s => s.OwnerId);
            
        }

        void ConfigureModelBuilderForLike(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>().ToTable("Like");
            modelBuilder.Entity<Like>().HasKey(l => new { l.StoryId, l.UserId });
        }
    }
}