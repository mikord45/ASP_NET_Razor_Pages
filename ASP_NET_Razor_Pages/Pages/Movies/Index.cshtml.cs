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
    public class IndexModel : PageModel
    {
        private readonly ASP_NET_Razor_Pages.Data.ASP_NET_Razor_PagesContext _context;

        public IndexModel(ASP_NET_Razor_Pages.Data.ASP_NET_Razor_PagesContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Movie != null)
            {
                Movie = await _context.Movie.ToListAsync();
            }
        }
    }
}
