using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yummly.Models.DbModels;

namespace Yummly.Models_config
{
    public class FollowConfiguration : IEntityTypeConfiguration<FollowModel>
    {
        public void Configure(EntityTypeBuilder<FollowModel> builder)
        {
            builder
                .HasKey(f => new { f.FollowingUserId, f.FollowedUserId });
                
            builder
                .HasOne(f => f.FollowingUser) 
                .WithMany(u => u.Followings)
                .HasForeignKey(f => f.FollowingUserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .HasOne(f => f.FollowedUser)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(f => f.FollowingUserId)
                .IsRequired();
            builder
                .Property(f=>f.FollowedUserId)
                .IsRequired();
            builder
                .Property (f => f.CreatedAt)
                .IsRequired();

        }
    }
}
