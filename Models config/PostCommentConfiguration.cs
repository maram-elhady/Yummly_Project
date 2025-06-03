using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yummly.Models.DbModels;

namespace Yummly.Models_config
{
    public class PostCommentConfiguration : IEntityTypeConfiguration<PostCommentModel>
    {
        public void Configure(EntityTypeBuilder<PostCommentModel> builder)
        {
            builder
               .HasKey(c => c.CommentId);
            builder
                .HasOne(c => c.User)
                .WithMany(u => u.PostComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(c => c.Post)
                .WithMany(p => p.PostComments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(c =>  c.PostId).IsRequired();
            builder
                .Property(c => c.UserId).IsRequired();
            builder
                .Property(c => c.Body).IsRequired();
        }
    }
}
