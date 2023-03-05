using ASP_NET_Razor_Pages.Data;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        public ApplicationDbContext context;
        [TestInitialize]
        public async Task initializeTest()
        {
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            this.context = new ApplicationDbContext(_contextOptions);
            this.context.Database.EnsureCreated();


            this.context.Rating.AddRange(
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
                });

            this.context.SaveChanges();

            this.context.AddRange(
                new ProductionCompany
                {
                    Name = "Warner Bros. Pictures"
                },
                new ProductionCompany
                {
                    Name = "Universal Pictures"
                },
                new ProductionCompany
                {
                    Name = "Walt Disney Pictures"
                },
                new ProductionCompany
                {
                    Name = "20th Century Studios"
                },
                new ProductionCompany
                {
                    Name = "Paramount Pictures"
                }
                );

            this.context.SaveChanges();

            var ratingQuery = from m in this.context.Rating orderby m.Name select m;
            var rating = await ratingQuery.ToListAsync();

            var productionCompaniesQuery = from m in this.context.ProductionCompany orderby m.Name select m;
            var productionCompany = await productionCompaniesQuery.ToListAsync();

            this.context.Movie.AddRange(
                new Movie
                {
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M,
                    Rating = rating[0],
                    ProductionCompany = productionCompany[0]
                },
                new Movie
                {
                    Title = "Ghostbusters",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M,
                    Rating = rating[1],
                    ProductionCompany = productionCompany[1]
                },
                new Movie
                {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Price = 9.99M,
                    Rating = rating[2],
                    ProductionCompany = productionCompany[2]
                },
                new Movie
                {
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Price = 3.99M,
                    Rating = rating[3],
                    ProductionCompany = productionCompany[3]
                }
            );
            this.context.SaveChanges();
        }

        [TestMethod]
        public async Task testIndexPage()
        {
            var indexPage = new ASP_NET_Razor_Pages.Models.Movies.IndexModel(this.context);

            await indexPage.OnGetAsync();
            var expectedGenresOptionsInSelect = "[{\"Disabled\":false,\"Group\":null,\"Selected\":false,\"Text\":\"Romantic Comedy\",\"Value\":null},{\"Disabled\":false,\"Group\":null,\"Selected\":false,\"Text\":\"Comedy\",\"Value\":null},{\"Disabled\":false,\"Group\":null,\"Selected\":false,\"Text\":\"Western\",\"Value\":null}]";
            Assert.AreEqual(expectedGenresOptionsInSelect, JsonConvert.SerializeObject(indexPage.Genres));

            Assert.AreEqual(4, indexPage.Movie.Count());
            Assert.AreEqual("When Harry Met Sally", indexPage.Movie[0].Title);
            Assert.AreEqual("Ghostbusters", indexPage.Movie[1].Title);
            Assert.AreEqual("Ghostbusters 2", indexPage.Movie[2].Title);
            Assert.AreEqual("Rio Bravo", indexPage.Movie[3].Title);

            indexPage.SearchString = "Ghost";
            await indexPage.OnGetAsync();
            Assert.AreEqual(2, indexPage.Movie.Count());
            Assert.AreEqual("Ghostbusters", indexPage.Movie[0].Title);
            Assert.AreEqual("Ghostbusters 2", indexPage.Movie[1].Title);
        }

        [TestMethod]
        public async Task testDetailsPage()
        {
            var detailsPage = new ASP_NET_Razor_Pages.Models.Movies.DetailsModel(this.context);

            await detailsPage.OnGetAsync(1);
            Assert.AreEqual("When Harry Met Sally", detailsPage.Movie.Title);
            Assert.AreEqual("Romantic Comedy", detailsPage.Movie.Genre);
            decimal price = (decimal)7.99;
            Assert.AreEqual(price, detailsPage.Movie.Price);
        }

        [TestMethod]
        public async Task testEditPage()
        {
            var editPage = new ASP_NET_Razor_Pages.Models.Movies.EditModel(this.context);

            await editPage.OnGetAsync(1);
            Assert.AreEqual("When Harry Met Sally", editPage.Movie.Title);
            Assert.AreEqual("Romantic Comedy", editPage.Movie.Genre);
            decimal price = (decimal)7.99;
            Assert.AreEqual(price, editPage.Movie.Price);

            editPage.Movie.Title = "When Harry Met Sally 2";
            await editPage.OnUpdateAsync();
            Assert.AreEqual("When Harry Met Sally 2", editPage.Movie.Title);
        }

        [TestMethod]
        public async Task testDeletePage()
        {
            var deletePage = new ASP_NET_Razor_Pages.Models.Movies.DeleteModel(this.context);

            await deletePage.OnGetAsync(1);
            Assert.AreEqual("When Harry Met Sally", deletePage.Movie.Title);
            Assert.AreEqual("Romantic Comedy", deletePage.Movie.Genre);
            decimal price = (decimal)7.99;
            Assert.AreEqual(price, deletePage.Movie.Price);

            deletePage.Movie.Title = "When Harry Met Sally";
            var result = await deletePage.OnDeleteAsync(1);
            var okResult = new OkResult();
            Assert.AreEqual(okResult.ToString(), result.ToString());
        }

        [TestMethod]
        public async Task testCreatePage()
        {
            var createPage = new ASP_NET_Razor_Pages.Models.Movies.CreateModel(this.context);

            var result = await createPage.OnGetAsync();

            createPage.Movie = new Movie
            {
                Title = "TestTitle",
                ReleaseDate = DateTime.Parse("1990-2-12"),
                Genre = "TestGenre",
                Price = 10.0M,
            };

            createPage.MovieRating = "3";
            createPage.MovieProductionCompany = "3";

            var result2 = await createPage.OnPostAsync();

            Console.WriteLine(JsonConvert.SerializeObject(result2));
            var indexPage = new ASP_NET_Razor_Pages.Models.Movies.IndexModel(this.context);
            await indexPage.OnGetAsync();
            Assert.AreEqual(5, indexPage.Movie.Count());
            Assert.AreEqual("TestTitle", indexPage.Movie[4].Title);
            Assert.AreEqual(DateTime.Parse("1990-2-12"), indexPage.Movie[4].ReleaseDate);
            Assert.AreEqual("TestGenre", indexPage.Movie[4].Genre);
            Assert.AreEqual(10.0M, indexPage.Movie[4].Price);
            Assert.AreEqual("PG-13", indexPage.Movie[4].Rating.Name);
            Assert.AreEqual("Walt Disney Pictures", indexPage.Movie[4].ProductionCompany.Name);

        }
    }
}