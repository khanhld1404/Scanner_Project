using Manage_PocketPc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Manage_PocketPc.Services;
namespace Manage_PocketPc.Controllers
{
    public class ScanController : Controller
    {
        public AppDbContext _db;
        private readonly ICsvImporter _importer;
        public ScanController(AppDbContext db, ICsvImporter importer = null)
        {
            _db = db;
            _importer = importer;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _db.ScanData.AsNoTracking().ToListAsync();
            return View(data);
        }

        // xử lý việc cập nhật dữ liệu

        public async Task<IActionResult> RunImport()
        {

            int rows = await _importer.ImportAllAsync(Cl_Connection.folder_data);

            TempData["success"] = "Số dòng dữ liệu đã thêm: " + rows;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string UserCode, string MasterCode, string DeviceNumber)
        {

            var user = (UserCode ?? string.Empty).Trim();
            var master = (MasterCode ?? string.Empty).Trim();
            var device = (DeviceNumber ?? string.Empty).Trim();


            ViewBag.UserScan = UserCode;
            ViewBag.MasterScan = MasterCode;
            ViewBag.DeviceScan = DeviceNumber;


            var query = _db.ScanData.AsNoTracking();

            bool chechk_user = string.IsNullOrWhiteSpace(user);
            bool chechk_master = string.IsNullOrWhiteSpace(master);
            bool chechk_device = string.IsNullOrWhiteSpace(device);

            if (!chechk_user)
            {
                query = query.Where(p => p.User_code.Contains(user));
            }

            if (!chechk_master)
            {
                query = query.Where(p => p.Master_code.Contains(master));
            }

            if (!chechk_device)
            {
                query = query.Where(p => p.DeviceNumber.Contains(device));
            }

            if(chechk_user && chechk_master && chechk_device)
            {
                TempData["error"] = "Nhập thông tin để bắt đầu tìm kiếm";
            }
            var ScanData = await query.ToListAsync();
            return View(ScanData);
        }

        public async Task<IActionResult> Delete(string Id)
        {


            var tt = await _db.ScanData
            .AsNoTracking()
            .FirstAsync(p => p.Id.ToString() == Id);

            _db.ScanData.Remove(tt);
            _db.SaveChanges();
            TempData["success"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }

    }
}
