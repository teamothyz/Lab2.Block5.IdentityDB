using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int SupplierId { get; set; }

        public int CategoryId { get; set; }

        public Supplier Supplier { get; set; } = null!;

        public Category Category { get; set; } = null!;
    }
}
