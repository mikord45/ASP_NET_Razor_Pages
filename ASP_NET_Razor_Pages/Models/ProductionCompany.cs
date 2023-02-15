using RazorPagesMovie.Models;
using RazorPagesMovie.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesMovie.Models { 
    public class ProductionCompany: IExternalMovieProperty
    {

        public int Id { get; set; }

        [StringLength(255, MinimumLength = 1)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public List<Movie> Movies { get; set; }
    }
}
