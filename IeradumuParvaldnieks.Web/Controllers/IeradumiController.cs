
using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using IeradumuParvaldnieks.Web.Services;
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
    }
}
