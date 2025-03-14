using E_Ticket.Models;
using E_Ticket.Repositories;
using E_Ticket.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticket.Areas.Admin.Controllers
{
[Area("Admin")]
[Authorize(Roles = "Admin,SuperAdmin,Company")]
public class CinemaAdminController : Controller
{

    private readonly ICinemaRepository cinemaRepository;

    public CinemaAdminController(ICinemaRepository cinemaRepository)
    {
        this.cinemaRepository = cinemaRepository;
    }

    public IActionResult Cinemas()
    {
        var cinemas = cinemaRepository.Get();
        return View(cinemas);
    }


    public ActionResult Create()
    {
        return View(new Cinema());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Cinema cinema, IFormFile file)
    {
        if (!ModelState.IsValid)
        {

            if (file != null && file.Length > 0)
            {

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Cinema", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }


                cinema.CinemaLogo = fileName;
            }

            cinemaRepository.Create(cinema);
            cinemaRepository.Commit();
            return RedirectToAction(nameof(Cinemas));
        }
        return View(cinema);
    }

    [HttpGet]
    public ActionResult Edit(int cinemaId, IFormFile file)
    {
        var cinema = cinemaRepository.GetOne(e => e.Id == cinemaId);
        if (cinema != null)
        {
            return View(cinema);
        }
        return RedirectToAction("NotFoundPage", "Cinemas");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Cinema cinema, IFormFile file)
    {
        var cinemaInDb = cinemaRepository.GetOne(e => e.Id == cinema.Id, tracked: false);
        if (!ModelState.IsValid)
        {
            if (cinemaInDb != null && file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Cinema", fileName);


                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Cinema", cinemaInDb.CinemaLogo);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Save img name in db
                cinema.CinemaLogo = fileName;

            }
            else
                cinema.CinemaLogo = cinemaInDb.CinemaLogo;

            if (cinema != null)
            {
                cinemaRepository.Edit(cinema);
                cinemaRepository.Commit();

                return RedirectToAction("Cinemas");
            }

        }
        return RedirectToAction("NotFoundPage", "Home");
    }



    public IActionResult DeleteImg(int cinemaId)
    {
        var cinema = cinemaRepository.GetOne(e => e.Id == cinemaId);

        if (cinema != null)
        {
            // Delete old img from wwwroot
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Cinema", cinema.CinemaLogo);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            // Delete img name in db
            cinema.CinemaLogo = null;
            cinemaRepository.Commit();

            return RedirectToAction("Edit", new { cinemaId });
        }

        return RedirectToAction("NotFoundPage", "Home");
    }



    [HttpGet]
    public IActionResult Details(int cinemaid)
    {
        var cinema = cinemaRepository.GetOne(e => e.Id == cinemaid);
        if (cinema != null)
        {
            return View(cinema);

        }
        return RedirectToAction("NotFoundPage", "Home");
    }





    public IActionResult Delete(int cinemaId)
    {
        var cinema = cinemaRepository.GetOne(e => e.Id == cinemaId);
        if (cinema != null)
        {

            if (cinema.CinemaLogo != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Cinema", cinema.CinemaLogo);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }


            cinemaRepository.Delete(cinema);
            cinemaRepository.Commit();

            return RedirectToAction("Cinemas");
        }

        return RedirectToAction("NotFoundPage", "Home");
    }





    public IActionResult NotFoundPage()
    {
        return View();
    }
}
}
