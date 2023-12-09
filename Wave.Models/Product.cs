using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wave.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Product Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MinLength(20, ErrorMessage ="Minimum description 20 character")]
        public string Description { get; set; }
        [DisplayName("List Price")]
        [Range(0, 2000)]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 2000)]
        public double Price { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [DisplayName("Product Availability")]
        public string? ProductAvailbality { get; set; }

        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }
    }
}
