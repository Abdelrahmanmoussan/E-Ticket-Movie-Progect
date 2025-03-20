using E_Ticket.Models;
using E_Ticket.Models.E_Ticket.Models;
using E_Ticket.Repositories;
using E_Ticket.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticket.Areas.Customers.Controllers
{
        [Area("Customers")]
    public class FavItemsController : Controller
    {
        private IMovieRepository _movieRepository;
        private IActorRepository _actorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ICinemaRepository _cinemaRepository;
        private ICategoryRepository _categoryRepository;
        private IFavItemRepository _favItemRepository;
        


        public FavItemsController(IMovieRepository movieRepository, IActorRepository actorRepository, ICinemaRepository cinemaRepository, ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager, FavItemRepository favItemRepository)
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
            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if not authenticated
            }

            var fav = new FavItems()
            {
                MovieId = MovieId,
                ApplicationUserId = user
            };

            _favItemRepository.Create(fav);
            _favItemRepository.Commit();

            return RedirectToAction("Index", "Movies", new { area = "Customers" });
        }


        public ActionResult FavList()
        {
            var userId = _userManager.GetUserId(User);
            var favMovies = _favItemRepository.Get(e => e.ApplicationUserId == userId, includes: [e => e.Movie]);

            if (!favMovies.Any())
            {
                return RedirectToAction("NotFoundPage");
            }

            return View(favMovies);
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
