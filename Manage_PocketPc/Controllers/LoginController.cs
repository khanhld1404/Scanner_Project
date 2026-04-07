using Manage_PocketPc.Models;
using Manage_PocketPc.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Claims;
namespace Manage_PocketPc.Controllers
{
    public class LoginController : Controller
    {
        public AppDbContext _db;

        public LoginController(AppDbContext db)
        {
            _db = db;
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Account), "Login");
        }


        // Trang đăng nhập
        [AllowAnonymous]
        public IActionResult Account()
        {
            return View();
        }

        //Kiểm tra thông tin đăng nhập

        [AllowAnonymous]
        public async Task<IActionResult> AccountLogin(string UserCode, string Password, string? returnUrl = null)
        {
            var user = (UserCode ?? string.Empty).Trim();
            var pass = (Password ?? string.Empty).Trim();

            // TODO: thay bằng truy vấn thực tế (và NÊN dùng hash mật khẩu)
            var u = _db.LoginUsers.AsNoTracking()
                     .FirstOrDefault(p => p.user_code == user
                                       && p.password == pass
                                       && p.department == "Admin");

            if (u == null)
            {
                TempData["error"] = "Mã nhân viên hoặc mật khẩu không đúng";
                return RedirectToAction(nameof(Account), new { returnUrl });
            }

            // 1) Tạo Claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, u.user_code),
        new Claim(ClaimTypes.Name, u.user_code),
    };

            // 2) Tạo Identity + Principal theo đúng cookie scheme
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // 3) Đăng nhập (ghi cookie)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false, // ghi nhớ phiên nếu muốn
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            // 4) Điều hướng (ưu tiên quay lại trang trước đó nếu có)
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            // Chuyển sang trang IK (Controller=IK, Action=IK)
            return RedirectToAction("IK", "IK");
        }


        public async Task<IActionResult> Login()
        {
            var data = await _db.LoginUsers.AsNoTracking().ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> Search_Account(string User_code)
        {
            var uc = (User_code ?? string.Empty).Trim();
            var query = _db.LoginUsers.AsNoTracking();
            if ( uc == "")
            {
                TempData["error"] = "Mời bạn nhập mã nhân viên để bắt đầu tìm kiếm";
            }
            else
            {
                query = query.Where(p => p.user_code.Contains(uc));
            }
            var account = await query.ToListAsync();
            ViewBag.User = User_code;
            return View(account);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([Bind("user_code,SimplePassword,department")] LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Thông tin nhập vào có vấn đề";
                return View("/Views/Login/Create.cshtml", user);
            }

            // Kiểm tra trùng mã nhân viên
            var exists = await _db.LoginUsers
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(p => p.user_code == user.user_code);

            if (exists != null)
            {
                TempData["error"] = "Tài khoản với mã nhân viên đã được tạo";
                return View("/Views/Login/Create.cshtml", user);
            }

            // Bắt buộc có mật khẩu ở simple_password
            if (string.IsNullOrWhiteSpace(user.SimplePassword))
            {
                ModelState.AddModelError(nameof(user.SimplePassword), "Mời bạn nhập mật khẩu");
                return View("/Views/Login/Create.cshtml", user);
            }

            // Hash PBKDF2-SHA1 và gán vào 'hash'
            user.password = Handle_Password.Hash(user.SimplePassword);

            _db.LoginUsers.Add(user);
            await _db.SaveChangesAsync();

            TempData["success"] = "Tạo tài khoản thành công!";
            return RedirectToAction("Login"); // đổi theo route login của bạn
        }


        public async Task<IActionResult> Update(string user)
        {
            LoginUser user_tt = await _db.LoginUsers
                    .FirstAsync(p => p.user_code == user);
            user_tt.password = null;
            return View(user_tt);
        }
        [HttpPost]
        public async Task<IActionResult> Update(LoginUser user)
        {

            if (ModelState.IsValid)
            {
                var tt = await _db.LoginUsers.AsNoTracking()
                         .FirstOrDefaultAsync(p => p.user_code == user.user_code);
                if (tt != null)
                {
                    if (string.IsNullOrWhiteSpace(user.SimplePassword))
                    {
                        user.password = tt.password;
                    }
                    else
                    {
                        user.password = Handle_Password.Hash(user.SimplePassword);
                    }
                    _db.LoginUsers.Update(user);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Cập nhật tài khoản thành công!";

                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["error"] = "Không tồn tại tài khoản!";
                    return View(user);
                }
            }
            else
            {
                TempData["error"] = "Có vài thứ đang bị lỗi!";
                return View("/Views/Login/Update.cshtml");
            }
        }

        public async Task<IActionResult> Delete(string user)
        {
            var tt = await _db.LoginUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.user_code == user);

            if (tt == null)
            {
                TempData["error"] = "Không tìm thấy user cần xóa";
            }
            else
            {
                _db.LoginUsers.Remove(tt);
                _db.SaveChanges();
                TempData["success"] = "Xóa thành công!";
            }
            return RedirectToAction("Login");
        }
    }
}
