using Movies.API.Model;
using System.Collections.Generic;
using System.Linq;

namespace Movies.API.Data
{
    public class MoviesContextSeed
    {
        public static void SeedAsync(MoviesAPIContext moviesAPIContext)
        {
            if (!moviesAPIContext.Movie.Any())
            {
                var movies = new List<Movie>
                {
                    new Movie{
                    Id = 1,
                    Genre = "Sci-fi",
                    Title = "Predator",
                    Rating =    "9.3",
                    ImageUrl = "images/src",
                    ReleaseDate = new System.DateTime(1987, 6, 12),
                    Owner = "alice"
                    },
                    new Movie{
                    Id = 2,
                    Genre = "Sci-fi",
                    Title = "Dune",
                    Rating =    "8.3",
                    ImageUrl = "images/src",
                    ReleaseDate = new System.DateTime(2021, 9, 15),
                    Owner = "alice"
                    },
                    new Movie{
                    Id = 3,
                    Genre = "Sci-fi",
                    Title = "Stargate",
                    Rating =    "8.3",
                    ImageUrl = "images/src",
                    ReleaseDate = new System.DateTime(1994, 10, 28),
                    Owner = "alice"
                    },
                };
                moviesAPIContext.Movie.AddRange(movies);
                moviesAPIContext.SaveChanges();
            }
        }
    }
}
