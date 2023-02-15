using RazorPagesMovie.Models;

namespace RazorPagesMovie.Models.Interfaces
{
    public interface IExternalMovieProperty
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public List<Movie> Movies { get; set; }


    }
}
