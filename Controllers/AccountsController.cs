
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projecttour.Extention;
using projecttour.Helpper;
using projecttour.Models;
using projecttour.ModelViews;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using BCrypt.Net;

/*namespace projecttour.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly DatabaseContext _context;
        public INotyfService _notyfService { get; }
        private readonly ILogger<AccountsController> _logger;
        private readonly object _userManager;
        private readonly object _signInManager;

        public AccountsController(DatabaseContext context, INotyfService notyfService)
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
       /*[Route("Acounts/tai-khoan-cua-toi.html", Name = "Dashboard")]
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
        }*/
       /* [HttpGet]
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
                        Salt = salt
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
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch
                    {
                        return RedirectToAction("DangkyTaiKhoan", "Accounts");
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
       
        /* public IActionResult Login(string returnUrl = null)
         {
             var taikhoanID = HttpContext.Session.GetString("UserId");
             if (taikhoanID != null)
             {
                 return RedirectToAction("Dashboard", "Accounts");
             }

             ViewData["ReturnUrl"] = returnUrl;
             return View();
           ; 
        }
        [HttpPost]
         [AllowAnonymous]
         [Route("dang-nhap.html", Name = "DangNhap")]
         public async Task<IActionResult> Login(LoginViewModel user, string returnUrl)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     bool isEmail = Utilities.IsValidEmail(user.UserName);
                     if (!isEmail) return View(user);

                     var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == user.UserName);

                     if (khachhang == null) return RedirectToAction("DangkyTaiKhoan");
                     string pass = (user.Password + khachhang.Salt.Trim()).ToMD5();
                     if (khachhang.Password != pass)
                     {
                         _notyfService.Success("Thông tin đăng nhập chưa chính xác");
                         return View(user);
                     }
                     //kiem tra xem account co bi disable hay khong

                     if (khachhang.Active == false)
                     {
                         return RedirectToAction("ThongBao", "Accounts");
                     }

                     //Luu Session MaKh
                     HttpContext.Session.SetString("UserId", khachhang.UserId.ToString());
                     var taikhoanID = HttpContext.Session.GetString("UserId");

                     //Identity
                     var claims = new List<Claim>
                     {
                         new Claim(ClaimTypes.Name, khachhang.FullName),
                         new Claim("UserId", khachhang.UserId.ToString())
                     };
                     ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                     ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                     await HttpContext.SignInAsync(claimsPrincipal);
                     _notyfService.Success("Đăng nhập thành công");
                     if (string.IsNullOrEmpty(returnUrl))
                     {
                         return RedirectToAction("Dashboard", "Accounts");
                     }
                     else
                     {
                         return Redirect(returnUrl);
                     }
                 }
             }
             catch
             {
                 return RedirectToAction("DangkyTaiKhoan", "Accounts");
             }
             return View(user);
         }*/

     


       /* [HttpGet]
        [Route("dang-xuat.html", Name = "DangXuat")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("UserID");
            return RedirectToAction("Index", "Home");
        }

        /*[HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.Users.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null) return RedirectToAction("Login", "Accounts");
                    var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
                    {
                        string passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
                        taikhoan.Password = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        _notyfService.Success("Đổi mật khẩu thành công");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                }
            }
            catch
            {
                _notyfService.Success("Thay đổi mật khẩu không thành công");
                return RedirectToAction("Dashboard", "Accounts");
            }
            _notyfService.Success("Thay đổi mật khẩu không thành công");
            return RedirectToAction("Dashboard", "Accounts");
        }*/
    //}
//}
