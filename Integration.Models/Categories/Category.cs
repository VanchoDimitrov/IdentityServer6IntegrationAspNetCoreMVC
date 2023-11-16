using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Integration.Models.Categories
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        public string DisplayOrder { get; set; }
        public DateTime CreatedDateTie { get; set; } = DateTime.Now;
    }
}