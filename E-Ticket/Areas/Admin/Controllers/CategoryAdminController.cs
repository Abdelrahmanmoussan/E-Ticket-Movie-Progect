
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

        [HttpGet]  
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {

                categoryRepository.Create(category);
                categoryRepository.Commit();
                return RedirectToAction("Category");
            }
            return View("NotFoundPage");
        }

        [HttpGet]
        public IActionResult Edit(int categoryId)
        {
            var category = categoryRepository.GetOne(e=>e.Id == categoryId);
            if (category != null)
            {

            return View(category);
            }
            return View("NotFoundPage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            var categories = categoryRepository.GetOne(e=> e.Id == category.Id, tracked: false);
            if (!ModelState.IsValid)
            {
                if (category != null)
                {
                    categoryRepository.Edit(category);
                    categoryRepository.Commit();

                }
                    return RedirectToAction("Category");
            }
                return RedirectToAction("NotFoundPage");
        }

        [HttpGet]
        public IActionResult Details(int categoryId)
        {
            var category = categoryRepository.GetOne(e=> e.Id==categoryId);

                if(category != null)
                {
                    return View(category);
                }
                return RedirectToAction("NotFoundPage");
        }

        public IActionResult Delete(int categoryId)
        {
            var category = categoryRepository.GetOne(e => e.Id == categoryId);
            if (category != null)
            {
                categoryRepository.Delete(category);
                categoryRepository.Commit();
                return RedirectToAction("Category");
            }
            return RedirectToAction("NotFoundPage");

        }






        public IActionResult NotFoundPage()
        {
            return View();
        }

    }

}

