using Microsoft.AspNetCore.Mvc;
using E_Ticket.DataAccess;
using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace E_Ticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin,Company")]
    public class MovieAdminController : Controller
    {

        private readonly IMovieRepository movieRepository;
        private readonly IActorRepository actorRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly ICategoryRepository categoryRepository;

        public MovieAdminController(IMovieRepository movieRepository, IActorRepository actorRepository, ICinemaRepository cinemaRepository, ICategoryRepository categoryRepository)
        {
            this.movieRepository = movieRepository;
            this.actorRepository = actorRepository;
            this.cinemaRepository = cinemaRepository;
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Movie()
        {
            var movies = movieRepository.Get(includes: [e => e.Category, e => e.Actors, e => e.Cinema]);
            return View(movies.ToList());
        }


        [HttpGet]
        public IActionResult Create()
        {
            var categories = categoryRepository.Get();
            ViewData["Categories"] = categories.ToList();

            var cinemas = cinemaRepository.Get();
            ViewBag.CinemaId = new SelectList(cinemas, "Id", "Name");

            var actors = actorRepository.Get();
            ViewBag.Actors = actors.ToList();


            return View(new Movie());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie, IFormFile? file, List<int> ActorIds)
        {

            ViewData["Categories"] = categoryRepository.Get().ToList();
            ViewBag.CinemaId = new SelectList(cinemaRepository.Get(), "Id", "Name");
            ViewBag.Actors = actorRepository.Get().ToList();

            if (!ModelState.IsValid)
            {

                if (file != null && file.Length > 0)
                {

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/movies", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }


                    movie.ImgUrl = fileName;
                }
                if (ActorIds != null && ActorIds.Any())
                {
                    var selectedActors = actorRepository.Get(a => ActorIds.Contains(a.Id)).ToList();
                    movie.Actors = selectedActors;
                }

                movieRepository.Create(movie);
                movieRepository.Commit();

                return RedirectToAction("Movie");
            }

            return View(movie);
        }

        [HttpGet]
        public IActionResult Edit(int movieId)
        {
            var movies = movieRepository.GetOne(e=> e.Id == movieId);
            var categories = categoryRepository.Get();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");


            var cinemas = cinemaRepository.Get();
            ViewBag.CinemaId = new SelectList(cinemas, "Id", "Name");

            var actors = actorRepository.Get();
            ViewBag.Actors = actors.ToList();

            if (movies != null)
            {
            return View(movies);

            }
            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie movie, IFormFile? file)
        {
            var movieInDb = movieRepository.GetOne(e => e.Id == movie.Id, tracked: false);
            if (!ModelState.IsValid)
            {

            if (movieInDb != null && file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);


                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", movieInDb.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Save img name in db
                movie.ImgUrl = fileName;
            }
            }
            else
                movie.ImgUrl = movieInDb.ImgUrl;

            if (movie != null)
            {
                movieRepository.Edit(movie);
                movieRepository.Commit();

                return RedirectToAction("Movie");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpGet]
        public IActionResult Details(int MovieId)
        {
            var movie = movieRepository.GetOne(e => e.Id == MovieId, includes: [m => m.Category, m => m.Cinema, m => m.Actors]);

            if (movie != null)
            {
                return View(movie);
            }
                return RedirectToAction(nameof(NotFoundPage));
        }


   

        public IActionResult DeleteImg(int MovieId)
        {
            var movie = movieRepository.GetOne(e => e.Id == MovieId);

            if (movie != null)
            {
                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", movie.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Delete img name in db
                movie.ImgUrl = null;
                movieRepository.Commit();

                return RedirectToAction("Edit", new { MovieId });
            }

            return RedirectToAction("NotFoundPage", "Home");
        }


        public IActionResult Delete(int MovieId)
        {
            var movie = movieRepository.GetOne(e => e.Id == MovieId);
            if (movie != null)
            {
                // Delete old img from wwwroot
                if (movie.ImgUrl != null)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", movie.ImgUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Delete img name in db
                movieRepository.Delete(movie);
                movieRepository.Commit();

                return RedirectToAction("Movie");
            }

            return RedirectToAction("NotFoundPage", "Home");

       
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }



    }

}
