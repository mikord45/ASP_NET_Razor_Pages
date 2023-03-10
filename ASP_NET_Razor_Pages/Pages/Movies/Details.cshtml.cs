using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ASP_NET_Razor_Pages.Data;
using RazorPagesMovie.Models;

namespace ASP_NET_Razor_Pages.Models.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly ASP_NET_Razor_Pages.Data.ApplicationDbContext _context;

        public DetailsModel(ASP_NET_Razor_Pages.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Movie Movie { get; set; }

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
            else 
            {
                Movie = movie;
            }
            return Page();
        }
    }
}
