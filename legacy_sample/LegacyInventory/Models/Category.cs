using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LegacyInventory.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Navigation property — one category has many products
        public virtual ICollection<Product> Products { get; set; }
    }
}
