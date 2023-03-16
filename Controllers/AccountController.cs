using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projecttour.Extention;
using projecttour.Helpper;
using projecttour.Models;
using projecttour.ModelViews;

namespace projecttour.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;
        public INotyfService _notyfService { get; }

        public AccountController(DatabaseContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x => x.PhoneNumber.ToLower() == Phone.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại : " + Phone + "đã được sử dụng");

                return Json(data: true);

            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email : " + Email + " đã được sử dụng");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult DangkyTaiKhoan()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> DangkyTaiKhoan(RegisterViewModel taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    User khachhang = new User
                    {
                        FullName = taikhoan.FullName,
                        PhoneNumber = taikhoan.PhoneNumber.Trim().ToLower(),
                        Email = taikhoan.Email.Trim().ToLower(),
                        Password = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Active = true,
                        Salt = salt,
                        Role = 3, // Set role là 3 (đây là ID của quyền truy cập "user")
                        Desciption = "user" // Set description là "user"
                    };

                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        //Lưu Session MaKh
                        HttpContext.Session.SetString("UserId", khachhang.UserId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("UserId");

                        //Identity
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,khachhang.FullName),
                    new Claim("UserId", khachhang.UserId.ToString())
                };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng ký thành công");
                        return RedirectToAction("Dashboard", "Account");
                    }
                    catch
                    {
                        return RedirectToAction("DangkyTaiKhoan", "Account");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginMV model)
        {
            if (ModelState.IsValid)
            {
                // Tìm user có email tương ứng
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);

                // Kiểm tra nếu user tồn tại và mật khẩu đúng
                if (user != null && VerifyPassword(model.Password, user.Password, user.Salt))
                {
                    // Kiểm tra role của user
                    if (user.Role == 1 || user.Role == 2)
                    {
                        // Lưu session cho user đã đăng nhập
                        HttpContext.Session.SetInt32("UserId", user.UserId);

                        // Đăng nhập vào area Admin
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else if (user.Role == 3)
                    {
                        // Lưu session cho user đã đăng nhập
                        HttpContext.Session.SetInt32("UserId", user.UserId);

                        // Đăng nhập vào Dashboarda
                        return RedirectToAction("Dashboard", "Account");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        // Hàm xác thực mật khẩu
        public bool VerifyPassword(string password, string savedPasswordHash, string salt)
        {
            // Tạo đối tượng HMACSHA512 từ salt
            string passwordHash = (password + salt).ToMD5();
            return savedPasswordHash == passwordHash;
        }

        [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("UserId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x => x.UserId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {

                    return View(khachhang);

                }

            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Route("dang-xuat.html", Name = "DangXuat")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }
    }
}
