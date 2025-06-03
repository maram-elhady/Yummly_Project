using System.ComponentModel.DataAnnotations;

namespace Yummly.Models.DbModels
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        //Nav property
        public ICollection<PostModel> Posts { get; set;}
    }
}
