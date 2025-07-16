using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Agri_Energy_Connect.Models
{
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }
        
        [Required(ErrorMessage = "Farmer name is required")]
        [Display(Name = "Farmer Name")]
        public string? FarmerName { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string? FarmerEmail { get; set; }
        
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string? FarmerPhone { get; set; }
        
        // Link to ASP.NET Identity User
        public string? UserId { get; set; }
        
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
        
        public ICollection<Product>? Products { get; set; }
    }
}

