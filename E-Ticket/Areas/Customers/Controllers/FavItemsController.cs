using E_Ticket.Models;
using E_Ticket.Models.E_Ticket.Models;
using E_Ticket.Repositories;
using E_Ticket.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace E_Ticket.Areas.Customers.Controllers
{
        [Area("Customers")]
        [Authorize]

    public class FavItemsController : Controller
    {
        private IMovieRepository _movieRepository;
        private IActorRepository _actorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ICinemaRepository _cinemaRepository;
        private ICategoryRepository _categoryRepository;
        private IFavItemRepository _favItemRepository;
        


        public FavItemsController(IMovieRepository movieRepository, IActorRepository actorRepository, ICinemaRepository cinemaRepository, ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager, IFavItemRepository favItemRepository)
        {
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
            _cinemaRepository = cinemaRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _favItemRepository = favItemRepository;
            


        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddToFav(int MovieId)
        {

            var user = _userManager.GetUserId(User);
            if (user != null)
            {

                var existingFav = _favItemRepository.Get(e => e.ApplicationUserId == user && e.MovieId == MovieId).FirstOrDefault();

                if (existingFav != null)
                {
                    // Movie is already in favorites, redirect to Index page
                    return RedirectToAction("Index", "Movies", new { area = "Customers" });
                }

                var fav = new FavItems()
                {
                    MovieId = MovieId,
                    ApplicationUserId = user
                };

                _favItemRepository.Create(fav);
                _favItemRepository.Commit();

            return RedirectToAction("FavList");
            }
                return RedirectToAction("Index", "Movies", new { area = "Customers" });
        }


        public ActionResult FavList()
        {
            var userId = _userManager.GetUserId(User);
            var favMovies = _favItemRepository.Get(e => e.ApplicationUserId == userId, includes: [e => e.Movie]);

            if (!favMovies.Any())
            {
                ViewBag.Message = "Your favorite list is empty.";
            }

            return View(favMovies);
        }


        public IActionResult Delete(int MovieId)
        {
            var user = _userManager.GetUserId(User);
            var favMovies = _favItemRepository.GetOne(e => e.ApplicationUserId == user && e.MovieId == MovieId);

            if(favMovies != null && user!= null)
            {

            _favItemRepository.Delete(favMovies);
            _favItemRepository.Commit();
            return RedirectToAction("FavList");
            }
            return RedirectToAction("NotFoundPage");

        }

        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
