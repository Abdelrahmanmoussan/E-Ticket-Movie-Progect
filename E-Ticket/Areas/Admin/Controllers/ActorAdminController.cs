using E_Ticket.Models;
using E_Ticket.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticket.Areas.Admin.Controllers
{
    [Area("Admin")]
public class ActorAdminController : Controller
{

    private readonly IMovieRepository movieRepository;
    private readonly IActorRepository actorRepository;
    private readonly ICinemaRepository cinemaRepository;
    private readonly ICategoryRepository categoryRepository;

    public ActorAdminController(IMovieRepository movieRepository, IActorRepository actorRepository, ICinemaRepository cinemaRepository, ICategoryRepository categoryRepository)
    {
        this.movieRepository = movieRepository;
        this.actorRepository = actorRepository;
        this.cinemaRepository = cinemaRepository;
        this.categoryRepository = categoryRepository;
    }

    public ActionResult ActorList()
    {
        var actors = actorRepository.Get();
        return View(actors);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View(new Actor());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Actor actor, IFormFile file)
    {
        if (!ModelState.IsValid)
        {

            if (file != null && file.Length > 0)
            {

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/cast", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }


                actor.ProfilePicture = fileName;
            }

            actorRepository.Create(actor);
            actorRepository.Commit();
            return RedirectToAction(nameof(ActorList));
        }
        return View(actor);
    }

    [HttpGet]
    public ActionResult Edit(int actorId, IFormFile file)
    {
        var actor = actorRepository.GetOne(e=> e.Id == actorId);
        if (actor != null)
        {
            return View(actor);
        }
        return RedirectToAction("NotFoundPage", "ActorList");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Actor actor, IFormFile file)
    {
        var actorInDb = actorRepository.GetOne(e => e.Id == actor.Id, tracked: false);
        if (!ModelState.IsValid)
        {
            if (actorInDb != null && file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);


                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", actorInDb.ProfilePicture);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                    // Save img name in db
                    actor.ProfilePicture = fileName;

            }
            else
                    actor.ProfilePicture = actorInDb.ProfilePicture;

            if (actor != null)
            {
                actorRepository.Edit(actor);
                actorRepository.Commit();

                return RedirectToAction("ActorList");
            }

        }
            return RedirectToAction("NotFoundPage", "Home");
    }



        public IActionResult DeleteImg(int actorId)
        {
            var actor = actorRepository.GetOne(e => e.Id == actorId);

            if (actor != null)
            {
                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", actor.ProfilePicture);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Delete img name in db
                actor.ProfilePicture = null;
                actorRepository.Commit();

                return RedirectToAction("Edit", new { actorId });
            }

            return RedirectToAction("NotFoundPage", "Home");
        }



        [HttpGet]
        public IActionResult Details(int actorid)
        {
            var actor = actorRepository.GetOne(e => e.Id == actorid);
            if (actor != null)
            {
                return View(actor);

            }
            return RedirectToAction("NotFoundPage", "Home");
        }





        public IActionResult Delete(int actorId)
        {
            var actor = actorRepository.GetOne(e => e.Id == actorId);
            if (actor != null)
            {
                
                if (actor.ProfilePicture != null)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", actor.ProfilePicture);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                
                actorRepository.Delete(actor);
                actorRepository.Commit();

                return RedirectToAction("ActorList");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }





        public IActionResult NotFoundPage()
    {
        return View();
    }
}
}
