using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASP_NET_Razor_Pages.Data;
using RazorPagesMovie.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Policy;

namespace ASP_NET_Razor_Pages.Models.Movies
{
    public class CreateModel : PageModel
    {
        private readonly ASP_NET_Razor_Pages.Data.ApplicationDbContext _context;

        public CreateModel(ASP_NET_Razor_Pages.Data.ApplicationDbContext context)
        {
            _context = context;

        }

        public string EmptyRating = "";
        public string EmptyProductionCompany = "";

        [BindProperty]
        public Movie Movie { get; set; }

        public SelectList? Ratings { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? MovieRating { get; set; }

        public SelectList? ProductionCompanies { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? MovieProductionCompany { get; set; }

        public async Task loadRatingsAndProductionCompanies()
        {
            var ratingsQuery = from m in _context.Rating select m;
            var allRatings = await ratingsQuery.ToListAsync();
            Ratings = new SelectList(allRatings, "Id", "Name");

            var productionCompaniesQuery = from m in _context.ProductionCompany select m;
            var allProductionCompanies = await productionCompaniesQuery.ToListAsync();
            ProductionCompanies = new SelectList(allProductionCompanies, "Id", "Name");
        }

        public void loadErrorMessagesOnEmptyRatingOrProductionCompany(bool emptyRating, bool emptyProductionCompany)
        {
            if (emptyRating)
            {
                EmptyRating = "Rating is required.";
            }
            if (emptyProductionCompany) 
            {
                EmptyProductionCompany = "Production Company is required";
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await loadRatingsAndProductionCompanies();
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Movie.Rating = _context.Rating.Where(r => r.Id.ToString() == MovieRating).FirstOrDefault();
            Movie.ProductionCompany = _context.ProductionCompany.Where(c => c.Id.ToString() == MovieProductionCompany).FirstOrDefault();
            bool ratingProvided = Movie.Rating != null;
            bool productionCompanyProvided = Movie.ProductionCompany != null;
            if (!ModelState.IsValid || !ratingProvided || !productionCompanyProvided)
            {
                loadErrorMessagesOnEmptyRatingOrProductionCompany(!ratingProvided, !productionCompanyProvided);
                await loadRatingsAndProductionCompanies();
                return Page();
            }

            _context.Movie.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
