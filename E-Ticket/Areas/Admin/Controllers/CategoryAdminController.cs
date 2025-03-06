
using Microsoft.AspNetCore.Mvc;
using E_Ticket.DataAccess;
using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Ticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryAdminController : Controller
    {

        private readonly IMovieRepository movieRepository;
        private readonly IActorRepository actorRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly ICategoryRepository categoryRepository;

        public CategoryAdminController(IMovieRepository movieRepository, IActorRepository actorRepository, ICinemaRepository cinemaRepository, ICategoryRepository categoryRepository)
        {
            this.movieRepository = movieRepository;
            this.actorRepository = actorRepository;
            this.cinemaRepository = cinemaRepository;
            this.categoryRepository = categoryRepository;
        }


        public IActionResult Category()
        {
            var category = categoryRepository.Get();
            return View(category.ToList());
        }

    }

}

