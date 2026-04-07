using System.Diagnostics;
using Manage_PocketPc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Manage_PocketPc.Controllers
{
    public class RFCController : Controller
    {
        public AppDbContext _db;

        public RFCController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]                
        public async Task<IActionResult> RFC()
        {
            var data = await _db.RFCs
                        .AsNoTracking()
                        .ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> Search_RFC(string AN_Search, string Product_Search)
        {

            var anRaw = (AN_Search ?? string.Empty).Trim();
            var productRaw = (Product_Search ?? string.Empty).Trim();

            var query = _db.RFCs.AsNoTracking().AsQueryable();

            if (anRaw != "" && productRaw != "")
            {
                query = query
                        .Where(p => p.an.ToString().Contains(anRaw) && p.product.Contains(productRaw));
                ViewBag.Keyword_AN = AN_Search;
                ViewBag.Keyword_Product = Product_Search;
            }
            else if (productRaw != "")
            {
                query = query
                    .Where(p => p.product.Contains(productRaw));
                ViewBag.Keyword_Product = Product_Search;
            }
            else if (anRaw != "")
            {
                query = query
                    .Where(p => p.an.ToString().Contains(anRaw));
                ViewBag.Keyword_AN = AN_Search;
            }
            else
            {
                TempData["error"] = "Mời bạn nhập an hoặc product để bắt đầu tìm kiếm!";
            }
            var RFC = await query.ToListAsync();
            return View(RFC);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RFC rfc)
        {
            if (ModelState.IsValid)
            {
                var tt = await _db.RFCs.AsNoTracking()
                         .FirstOrDefaultAsync(p => p.product == rfc.product);
                if (tt != null)
                {
                    TempData["error"] = "RFC với giá trị product đã có trong cơ sở dữ liệu";
                    return View("/Views/IK/Create.cshtml");
                }
                else
                {
                    _db.RFCs.Add(rfc);
                    await _db.SaveChangesAsync();

                    TempData["success"] = "Thêm RFC thành công!";
                    return RedirectToAction("RFC");
                }
            }
            else
            {
                TempData["error"] = "Có vài thứ đang bị lỗi!";
                return View("/Views/RFC/Create.cshtml");
            }
        }


        public async Task<IActionResult> Update(string product)
        {
            RFC rfc = await _db.RFCs
                    .FirstAsync(p => p.product == product);
            return View(rfc);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RFC rfc)
        {

            if (ModelState.IsValid)
            {
                var tt = await _db.RFCs.AsNoTracking()
                         .FirstOrDefaultAsync(p => p.product == rfc.product);
                if (tt != null)
                {
                    _db.RFCs.Update(rfc);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Cập nhật RFC thành công!";

                    return RedirectToAction("RFC");
                }
                else
                {
                    TempData["error"] = "Không tồn tại RFC!";
                    return View(rfc);
                }
            }
            else
            {
                TempData["error"] = "Có vài thứ đang bị lỗi!";
                return View("/Views/RFC/Update.cshtml");
            }
        }


        public async Task<IActionResult> Delete(string product)
        {
            var tt = await _db.RFCs
            .AsNoTracking()
            .FirstAsync(p => p.product == product);

            _db.RFCs.Remove(tt);
            _db.SaveChanges();
            TempData["success"] = "Xóa thành công!";
            return RedirectToAction("RFC");
        }

    }
}
