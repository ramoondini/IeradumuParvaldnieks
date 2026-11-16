
using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using IeradumuParvaldnieks.Web.Services;
using IeradumuParvaldnieks.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace IeradumuParvaldnieks.Web.Controllers
{
    public class IeradumiController : Controller
    {
        private readonly IeradumuApiService apiService;

        public IeradumiController(IeradumuApiService apiService)
        {
            this.apiService = apiService;
        }

        // Ielade un parada aktivus ieradumus
        public async Task<IActionResult> Index()
        {
            try
            {
                var ieradumi = await apiService.IegutVisuAsync("user1");

                var aktivie = ieradumi
                    .Where(i => i.Statuss == IeradumaStatuss.Aktivs)
                    .ToList();

                return View(aktivie);
            }
            catch (HttpRequestException)
            {
                ViewBag.Kluda = "Neizdevās savienoties ar API serveri.";
                return View(new List<Ieradums>());
            }
            catch (TaskCanceledException)
            {
                ViewBag.Kluda = "API serveris neatbild.";
                return View(new List<Ieradums>());
            }
        }

        //Parada izveleta ieraduma detalas
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var ieradums = await apiService.IegutPecIdAsync(id);
                if (ieradums == null)
                    return NotFound();
                return View(ieradums);

            }
            catch (HttpRequestException)
            {
                return RedirectToAction(nameof(Index));
            }
            catch (TaskCanceledException)
            {
                return RedirectToAction(nameof(Index));
            }

        }

        //Parada jaunu ieraduma formu
        [HttpGet]
        public IActionResult Create()
        {
            return View(new IeradumaIzveidesModelis());
        }

        //Saglaba jaunu ieradumu caur API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IeradumaIzveidesModelis modelis)
        {
            if(!ModelState.IsValid)
                return View(modelis);

            try
            {
                var Ieradums = new Ieradums
                {
                    Id = Guid.NewGuid(),
                    LietotajaId = "user1",
                    Nosaukums = modelis.Nosaukums.Trim(),
                    Apraksts = modelis.Apraksts,
                    Statuss = IeradumaStatuss.Aktivs,
                    Izveidots = DateTime.Now,
                    Atjauninats = DateTime.Now,
                    Atkartosana = new Atkartosana
                    {
                        Biezums = modelis.Biezums,
                        Intervals = 1
                    }
                };

                await apiService.IzveidotAsync(Ieradums);

                TempData["Pazinojums"] = "Ieradums veiksmīgi izveidots.";
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("", "Neizdevās saglabāt ieradumu API serverī.");
            }
            catch (TaskCanceledException)
            {
                ModelState.AddModelError("", "API serveris neatbild.");
            }
            
            return View(modelis);
        }
    }
}
