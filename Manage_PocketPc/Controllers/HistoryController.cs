using Microsoft.AspNetCore.Mvc;
using Manage_PocketPc.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Manage_PocketPc.Controllers
{
    public class HistoryController : Controller
    {
        public AppDbContext _db;
        public HistoryController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var data = _db.UpdateHistories.AsNoTracking()
                .ToList();
            return View(data);
        }

        public IActionResult Search(string tt1, string tt2)
        {
            int version;
            var get_version = (tt1 ?? string.Empty).Trim();
            var person = (tt2 ?? string.Empty).Trim();

            var query = _db.UpdateHistories.AsNoTracking();

            if(get_version == "" && person == "")
            {
                return RedirectToAction("Index");
            }else if(get_version == "")
            {
                ViewBag.Person = person;
                query = query.Where(p => p.Person.Contains(person));
            }
            else if(person == "")
            {
                if (!int.TryParse(get_version, out version))
                {
                    TempData["error"] = "Bạn cần nhập số hợp lệ";
                    return RedirectToAction("Index");
                }
                ViewBag.Version = version;
                query = query.Where(p => p.Version == version);
            }
            else
            {
                if (!int.TryParse(get_version, out version))
                {
                    TempData["error"] = "Bạn cần nhập số hợp lệ";
                    return RedirectToAction("Index");
                }
                ViewBag.Person = person;
                ViewBag.Version = version;
                query = query.Where(p => p.Version == version && p.Person.Contains(person));
            }
            var search_data = query.ToList();
            return View(search_data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UpdateHistory up)
        {
            return View();
        }
    }
}
