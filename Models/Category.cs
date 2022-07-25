using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models
{
    /// <summary>
    /// Represents a category
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Category Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        [SwaggerSchema("Id", ReadOnly = true)]
        public int Id { get; set; }

        /// <summary>
        /// Name of the category
        /// </summary>
        [Required]
        public string Name { get; set; }

        public override bool Equals(object? obj) =>
            obj is Category x
            && Id == x.Id;

        public override int GetHashCode() => Id.GetHashCode();
    }
}
