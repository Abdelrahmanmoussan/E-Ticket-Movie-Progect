using E_Ticket.DataAccess;
using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Ticket.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly IActorRepository actorRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly ICategoryRepository categoryRepository;

        public MoviesController(IMovieRepository movieRepository, IActorRepository actorRepository, ICinemaRepository cinemaRepository, ICategoryRepository categoryRepository)
        {
            this.movieRepository = movieRepository;
            this.actorRepository = actorRepository;
            this.cinemaRepository = cinemaRepository;
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var movies = movieRepository.Get();
            if (movies != null)
            {
            return View(movies.ToList());
            }
            return RedirectToAction(nameof(NotFoundPage));
        }

        [HttpGet]
        public IActionResult Details(int MovieId)
        {
            var movie = movieRepository.GetOne(e => e.Id == MovieId, includes: [m => m.Category, m => m.Cinema, m => m.Actors]);

            if (movie == null)
            {
                return RedirectToAction(nameof(NotFoundPage));
            }

            
            ViewBag.RelatedMovies = movieRepository.Get(includes: [m => m.Actors])
                .Where(m => m.CategoryId == movie.CategoryId && m.Id != movie.Id) 
                .GroupBy(m => m.Id)
                .Select(g => g.First()) 
                .ToList();

            return View(movie);
        }


        [HttpGet]
        public IActionResult Category(int? cMovie)  
        {
            if (cMovie.HasValue)
            {
                
                var category = categoryRepository.GetOne(
                    e => e.Id == cMovie.Value, includes: [e => e.Movies]);

                if (category == null)
                {
                    return RedirectToAction(nameof(NotFoundPage));
                }

                return View("CategoryMovies", category.Movies.ToList());
            }

            var categories = categoryRepository.Get();
            return View(categories.ToList());
        }

        [HttpGet]
        public IActionResult Cinema(int? cMovieId)
        {
            if (cMovieId.HasValue)
            {

                var cinema = cinemaRepository.GetOne(
                    e => e.Id == cMovieId.Value, includes: [e => e.Movies]);

                if (cinema == null)
                {
                    return RedirectToAction(nameof(NotFoundPage));
                }

                return View("CinemaMovies", cinema.Movies.ToList()); 
            }

            var cinemas = cinemaRepository.Get();
            return View(cinemas.ToList());
        }


        [HttpGet]
        public IActionResult ActorDetails(int actorId)
        {
            var actor = actorRepository.GetOne(e => e.Id == actorId, includes: [a => a.Movies]);

            if (actor == null)
            {
                return RedirectToAction(nameof(NotFoundPage));
            }

            return View(actor);
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

    }
}