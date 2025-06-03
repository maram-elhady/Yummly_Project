using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yummly.Models.DbModels;
using Yummly.Models_config;

namespace MyRecipeApp.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)  { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FollowConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostLikeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostCommentConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly);


            modelBuilder.Entity<IdentityUserLogin<string>>()
               .HasKey(login => new { login.UserId, login.LoginProvider, login.ProviderKey });

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>()
                 .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });


            //seeding
            modelBuilder.Entity<CategoryModel>().HasData(
                 new CategoryModel { Id = 1, Name = "Breakfast", Photo = "/CategoryImages/breakfast.jpg" },
                 new CategoryModel { Id = 2, Name = "Lunch", Photo = "/CategoryImages/lunch.jpg" },
                 new CategoryModel { Id = 3, Name = "Dinner", Photo = "/CategoryImages/dinner.jpg" },
                 new CategoryModel { Id = 4, Name = "Dessert", Photo = "/CategoryImages/dessert.jpg" },
                 new CategoryModel { Id = 5, Name = "Beverages", Photo = "/CategoryImages/beverages.jpg" },
                 new CategoryModel { Id = 6, Name = "Seafood", Photo = "/CategoryImages/seafood.jpg" },
                 new CategoryModel { Id = 7, Name = "Snacks", Photo = "/CategoryImages/snacks.jpg" },
                 new CategoryModel { Id = 8, Name = "Healthy", Photo = "/CategoryImages/healthy.jpg" }
          );

        }
        public DbSet<FollowModel> follows { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<PostLikeModel> PostLikes { get; set; }
        public DbSet<PostCommentModel> PostComments { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }

    }
}