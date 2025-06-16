using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri_Energy_Connect.Models
{
    public class Product
    {
        [Key]
        public int ProdId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string? ProdName { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string? ProdDescription { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Price")]
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; } 

        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Date")]
        public DateTime? DateTime { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }
        public int? FarmerId { get; set; }
        public Farmer? Farmer { get; set; }
        //Slug read only
        public string Slug => $"{Category}-{ProdName}-{Price}".ToLower().Replace("-", "-");
    }
}
