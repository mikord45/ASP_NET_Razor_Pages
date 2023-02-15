using Microsoft.EntityFrameworkCore;
using ASP_NET_Razor_Pages.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace RazorPagesMovie.Models;

public static class SeedData
{
    public static async void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {
            if (context == null || context.Movie == null)
            {
                throw new ArgumentNullException("Null RazorPagesMovieContext");
            }

            // Look for any movies.
            if (context.Movie.Any() || context.Rating.Any())
            {
                return;   // DB has been seeded
            }

            context.AddRange(
                new Rating
                {
                    Name = "G",
                },
                new Rating
                {
                    Name = "PG",
                },
                new Rating
                {
                    Name = "PG-13",
                },
                new Rating
                {
                    Name = "R",
                },
                new Rating
                {
                    Name = "NC-17"
                }
            );

            context.SaveChanges();

            var ratingQuery = from m in context.Rating orderby m.Name select m;
            var rating = await ratingQuery.ToListAsync();
            //System.Diagnostics.Debug.WriteLine("RATINGS: ", JsonConvert.SerializeObject(rating));
            context.Movie.AddRange(
                new Movie
                {
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M,
                    Rating = rating[0]
                },
                new Movie
                {
                    Title = "Ghostbusters ",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M,
                    Rating = rating[1]
                },
                new Movie
                {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Price = 9.99M,
                    Rating = rating[2]
                },
                new Movie
                {
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Price = 3.99M,
                    Rating = rating[3]
                }
            );
            context.SaveChanges();
        }
    }
}