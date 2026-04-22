using Manage_PocketPc.Models;
using Manage_PocketPc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace Manage_PocketPc.Controllers
{
    public class ConnectController : Controller
    {
        private AppDbContext _db;
        public ConnectController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var tt = await _db.Keyence_versions.AsNoTracking()
                    .OrderByDescending(p => p.Version)
                    .ToListAsync();
            return View(tt);
        }

        [HttpPost]
        public IActionResult Create(string note)
        {

            //Đổi dữ kiệu từ sqlserver sang file sdf 
            // Đường dẫn đến file exe
            var exePath = Cl_Connection.exePath;
            // Đường dẫn đến file sdf
            var sdfPath = Cl_Connection.sdfPath;


            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"\"{sdfPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                var error = process.StandardError.ReadToEnd();
                return BadRequest("Tạo SDF thất bại:\n" + error);
            }

            if (!System.IO.File.Exists(sdfPath))
                return NotFound("Không tìm thấy file SDF sau khi tạo");

            // lưu version vào sqlserver
            Keyence_version tt = new Keyence_version();
            tt.Comment = note;
            _db.Keyence_versions.Add(tt);
            _db.SaveChanges();

            TempData["success"] = "Tạo kết nối thành công!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string version)
        {
            var pb = (version ?? "").Trim();
            var query = _db.Keyence_versions.AsNoTracking();
            if(pb == "")
            {
                TempData["error"] = "Mời bạn nhập phiên bản để bắt đầu tìm kiếm!";
            }
            else
            {
                if (int.TryParse(pb, out int value))
                {
                    query = query.Where(x => x.Version == value);
                }
                else
                {
                    TempData["error"] = "Nhập phiên bản không hợp lệ!";
                }
            }
            var data = await query.ToListAsync();
            ViewBag.Version = version;
            return View(data);
        }
    }
}
