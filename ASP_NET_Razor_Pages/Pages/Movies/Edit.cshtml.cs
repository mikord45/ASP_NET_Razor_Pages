using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP_NET_Razor_Pages.Data;
using RazorPagesMovie.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ASP_NET_Razor_Pages.Models.Movies
{
    public class EditModel : PageModel
    {
        private readonly ASP_NET_Razor_Pages.Data.ApplicationDbContext _context;

        public EditModel(ASP_NET_Razor_Pages.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;


        public SelectList? Ratings { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? MovieRating { get; set; }

        public SelectList? ProductionCompanies { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? MovieProductionCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.Include("Rating").Include("ProductionCompany").FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            var ratingsQuery = from m in _context.Rating select m;
            var allRatings = await ratingsQuery.ToListAsync();
            Ratings = new SelectList(allRatings, "Id", "Name");
            MovieRating = movie.Rating.Id.ToString();

            var productionCompaniesQuery = from m in _context.ProductionCompany select m;
            var allProductionCompanies = await productionCompaniesQuery.ToListAsync();
            ProductionCompanies = new SelectList(allProductionCompanies, "Id", "Name");
            MovieProductionCompany = movie.ProductionCompany.Id.ToString();

            Movie = movie;
            return Page();
        }

        public async Task<IActionResult> OnUpdateAsync()
        {
            Movie.Rating = _context.Rating.Where(r => r.Id.ToString() == MovieRating).FirstOrDefault();
            Movie.ProductionCompany = _context.ProductionCompany.Where(c => c.Id.ToString() == MovieProductionCompany).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            _context.Attach(Movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new OkResult();
        }

        private bool MovieExists(int id)
        {
          return _context.Movie.Any(e => e.Id == id);
        }
    }
}
