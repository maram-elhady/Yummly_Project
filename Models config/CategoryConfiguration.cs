using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yummly.Models.DbModels;

namespace Yummly.Models_config
{  
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryModel>
    {
        
        public void Configure(EntityTypeBuilder<CategoryModel> builder)
        {
            

            builder
                .HasKey(c => c.Id);

            builder
                .HasMany(c => c.Posts)
                .WithOne(p => p.Category)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .Property(p => p.Id)
                .IsRequired();
            builder
                .Property(p => p.Name)
                .IsRequired();


          
        }
    }
}
