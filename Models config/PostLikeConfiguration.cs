using Microsoft.EntityFrameworkCore;
using Yummly.Models.DbModels;

namespace Yummly.Models_config
{
    public class PostLikeConfiguration : IEntityTypeConfiguration<PostLikeModel>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PostLikeModel> builder)
        {
            builder
                .HasKey(l => new { l.PostId, l.UserId });
            builder
                .HasOne(l=>l.User)
                .WithMany(u=>u.PostLikes)
                .HasForeignKey(l=>l.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(l => l.Post)
                .WithMany(p => p.PostLikes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(l =>  l.PostId).IsRequired();

            builder
                .Property (l => l.UserId).IsRequired();
        }
    }
}
