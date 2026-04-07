using System.Diagnostics;
using Manage_PocketPc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Manage_PocketPc.Controllers
{
    public class IKController : Controller
    {
        public AppDbContext _db;
        public IKController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public async Task<IActionResult> IK()
        {

            Debug.WriteLine($"IsAuthenticated: {User?.Identity?.IsAuthenticated}");
            var data = await _db.IKs
                        .AsNoTracking()
                        .ToListAsync();
            return View(data);
        }


        public async Task<IActionResult> Search_IK(string AN_Search, string Product_Search)
        {

            var anRaw = (AN_Search ?? string.Empty).Trim();
            var productRaw = (Product_Search ?? string.Empty).Trim();

            var query = _db.IKs.AsNoTracking().AsQueryable();

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
            else if (anRaw !="")
            {
                query = query
                    .Where(p => p.an.ToString().Contains(anRaw));
                ViewBag.Keyword_AN = AN_Search;
            }
            else
            {
                TempData["error"] = "Mời bạn nhập an hoặc product để bắt đầu tìm kiếm!";
            }
            var IK = await query.ToListAsync();
            return View(IK);
        }

        public async Task<IActionResult> Update(string product)
        {
            IK ik = await _db.IKs
                    .FirstAsync(p => p.product == product);
            return View(ik);
        }
        [HttpPost]
        public async Task<IActionResult> Update(IK ik)
        {

            if (ModelState.IsValid)
            {
                var tt = await _db.IKs.AsNoTracking()
                         .FirstOrDefaultAsync(p => p.product == ik.product);
                if (tt != null)
                {
                    _db.IKs.Update(ik);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Cập nhật IK thành công!";

                    return RedirectToAction("IK");
                }
                else
                {
                    TempData["error"] = "Không tồn tại IK!";
                    return View(ik);
                }
            }
            else
            {
                TempData["error"] = "Có vài thứ đang bị lỗi!";
                return View("/Views/IK/Update.cshtml");
            }
        }

        public async Task<IActionResult> Delete_IK(string product)
        {
            var tt = await _db.IKs
            .AsNoTracking()
            .FirstAsync(p => p.product == product);

            _db.IKs.Remove(tt);
            _db.SaveChanges();
            TempData["success"] = "Xóa thành công!";
            return RedirectToAction("IK");
        }


        public IActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Create(IK ik)
        {
            if (ModelState.IsValid)
            {
                var tt = await _db.IKs.AsNoTracking()
                         .FirstOrDefaultAsync(p => p.product == ik.product);
                if(tt != null)
                {
                    TempData["error"] = "Ik với giá trị product đã có trong cơ sở dữ liệu";
                    return View("/Views/IK/Create.cshtml");
                }
                else
                {
                    _db.IKs.Add(ik);
                    await _db.SaveChangesAsync();

                    TempData["success"] = "Thêm IK thành công!";
                    return RedirectToAction("IK");
                }
            }
            else
            {
                TempData["error"] = "Có vài thứ đang bị lỗi!";
                return View("/Views/IK/Create.cshtml");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
