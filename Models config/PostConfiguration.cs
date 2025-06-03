using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yummly.Models.DbModels;

namespace Yummly.Models_config
{
    public class PostConfiguration : IEntityTypeConfiguration<PostModel>
    {
        public void Configure(EntityTypeBuilder<PostModel> builder)
        {
            builder
                .HasKey(p => p.Id);
            builder
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.PostComments)
                .WithOne(c => c.Post)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(p => p.PostLikes)
                .WithOne(l => l.Post)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(p => p.UserId).IsRequired();
            builder
                .Property(p => p.Title).IsRequired()
                .HasMaxLength(100);
            builder
                .Property(p => p.Description)
                .HasMaxLength(500);
            builder
                .Property(p=> p.ImageUrl).IsRequired();
            builder
                .Property(p => p.Link)
                .IsRequired(false);
            builder
                .Property(p => p.CreatedAt).IsRequired();
            
                
            

        }
    }
}
