using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ASP_NET_Razor_Pages.Data;
using RazorPagesMovie.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ASP_NET_Razor_Pages.Models.Movies
{
    [Authorize(Roles = "Admin, Member")]
    public class IndexModel : PageModel
    {
        private readonly ASP_NET_Razor_Pages.Data.ApplicationDbContext _context;

        public IndexModel(ASP_NET_Razor_Pages.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MovieGenre { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
                         select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.Include("Rating").Include("ProductionCompany").ToListAsync();
        }
    }
}
