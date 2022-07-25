using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models
{
    /// <summary>
    /// Represents an Item
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Item Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        [SwaggerSchema("Id",ReadOnly = true)]
        public int Id { get; set; }

        /// <summary>
        /// Name of the item
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Category of the item
        /// </summary>
        public Category Category { get; set; }

        public override bool Equals(object? obj) =>
            obj is Item x
            && Id == x.Id;

        public override int GetHashCode() => Id.GetHashCode();
    }
}
