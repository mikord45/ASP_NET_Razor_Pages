using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RazorPagesMovie.Models
{
    public class Rating
    {
        public int Id { get; set; }

        [StringLength(25, MinimumLength = 1)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public List<Movie> Movies { get; set; }

   }
}
