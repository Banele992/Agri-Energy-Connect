using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string? CatName { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}