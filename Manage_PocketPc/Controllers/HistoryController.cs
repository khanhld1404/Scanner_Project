using Microsoft.AspNetCore.Mvc;
using Manage_PocketPc.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Manage_PocketPc.Services;
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
                .OrderByDescending(p => p.Version)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            UpdateHistory data,
            List<IFormFile> uploadFiles
        )
        {
            // 1️⃣ Validate model
            if (ModelState.IsValid)
            {
                string folderPath = Cl_Connection.folder_data;

                if (uploadFiles == null || uploadFiles.Count == 0)
                {
                    return BadRequest("Cần nhập file thông tin!");
                }
                // 2️⃣ Kiểm tra trùng phiên bản (PHẢI await)
                var exist = await _db.UpdateHistories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Version == data.Version);

                if (exist != null)
                {
                    return BadRequest("Phiên bản đã tồn tại, không thể sử dụng.");
                }

                // Nhập dữ liệu txt
                CsvExporter.WriteToFile(data.Version.ToString(), folderPath, "Keyence_Program_Version.txt");

                // 3️⃣ Lưu dữ liệu
                data.Time = DateTime.Now;
                _db.UpdateHistories.Add(data);
                await _db.SaveChangesAsync();


                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                foreach (var file in uploadFiles)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(
                            folderPath,
                            Path.GetFileName(file.FileName)
                        );

                        using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);
                    }
                }
            }else
            {
                return BadRequest("Nhập thiếu dữ liệu!");
            }

            // 5️⃣ Trả kết quả về cho fetch
            return Ok("Thêm phiên bản thành công!");
        }
    }
}
