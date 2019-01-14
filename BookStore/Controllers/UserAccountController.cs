using BookStore.Data.Models;
using BookStore.Services;
using BookStore.SharedComponents;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("[controller]/[action]")]
    public class UserAccountController : Controller
    {

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<ApplicationUser> _signInManager;


        public UserAccountController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("/tai-khoan/{id?}")]
        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {

            var infor = await _userManager.FindByIdAsync(id);
            return View(infor);
        }
      
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, bool rememberMe)
        {
            string message = string.Empty;
            bool status = false;
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                message = "Tên đăng nhập hoặc mật khẩu không được để trống";
                return Json(new
                {
                    message = message,
                    status = status
                });
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                if (user.Status==false)
                {
                    if (user.Status == false)
                    {
                        return Json(new
                        {
                            message = "Tài khoản đã bị khóa bởi quản trị viên vì một lý do nào đó!",
                            status = false
                        });
                    }
                }else
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
                    if (result.Succeeded)
                    {
                        return Json(new
                        {
                            message = "Đăng nhập thành công",
                            status = true
                        });
                    }
                    else if (result.IsNotAllowed)
                    {
                        return Json(new
                        {
                            message = "Tài khoản chưa được kích hoạt. Hãy kiểm tra email để kích hoạt tài khoản",
                            status = false
                        });
                    }
                }
            }
            message = "Tên đăng nhập hoặc mật khẩu không đúng.";
            return Json(new
            {
                message = message,
                status = status
            });
        }
        [Route("/login")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModels()
            {
                ReturnUrl = returnUrl
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/login")]
        public async Task<IActionResult> Login(LoginViewModels loginViewModels)
        {
            if (!ModelState.IsValid)
                return View(loginViewModels);
            var user = await _userManager.FindByNameAsync(loginViewModels.UserName);

            if (user != null)
            {
                await _signInManager.SignOutAsync();
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModels.Password, loginViewModels.RememberMe, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginViewModels.ReturnUrl))
                    {
                        return View();
                    }
                    return Redirect(loginViewModels.ReturnUrl);
                }
            }
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(loginViewModels);
        }


        public IActionResult GoogleSignIn()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" });
        }

        [HttpPost]
        public JsonResult SendPasswordResetLink(string email)
        {
            string message = string.Empty;
            var user = _userManager.FindByEmailAsync(email).Result;

            if (user == null || !(_userManager.IsEmailConfirmedAsync(user).Result))
            {
                message = "Không tồn tại email hoặc tài khoản không được xác minh!";
                return Json(new
                {
                    status = false,
                    message = message
                });
            }

            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;

            var resetLink = Url.Action("ResetPassword",
                            "UserAccount", new { token = token },
                             protocol: HttpContext.Request.Scheme);

        
            try
            {
                EmailSender.SendMail(user.Email, resetLink);
                message = "Đường dẫn khôi phục mật khẩu đã được gởi đến email!";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new
            {
                status = true,
                message = message
            });
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel obj)
        {
            var user = _userManager.
                         FindByNameAsync(obj.UserName).Result;

            IdentityResult result = _userManager.ResetPasswordAsync
                      (user, obj.Token, obj.Password).Result;
            if (result.Succeeded)
            {
                ViewBag.Message = "Khôi phục mật khẩu thành công!";
                return View();
            }
            else
            {
                ViewBag.Message = "Lỗi khôi phục! Hãy chắc chắn là bạn nhập đúng tên tài khoản";
                return View();
            }
        }
        [HttpPost]
        public async Task<JsonResult> Register(string models)
        {
            var status = false;
            string message = string.Empty;
            var data = JsonConvert.DeserializeObject<LoginViewModels>(models);
            List<string> errorMessasge = new List<string>();
            ApplicationUser user = new ApplicationUser
            {
                UserName = data.UserName,
                Email = data.Email,
                CreatedDate=DateTime.Now,
                Status=true
            };
            IdentityResult result = await _userManager.CreateAsync(user, data.Password);
            if (result.Succeeded)
            {
                string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

                string confirmationLink = Url.Action("ConfirmEmail",
                  "UserAccount", new
                  {
                      userid = user.Id,
                      token = confirmationToken
                  },
                   protocol: HttpContext.Request.Scheme);
                try
                {
                EmailSender.SendMail(user.Email, confirmationLink);
                    status = true;
                    message = "Kiểm tra email để xác thực tài khoản";
                }catch(Exception ex)
                {
                    message = "Lỗi xác thực:"+ ex.Message;
                    status = false;
                }
                return Json(new
                {
                    status = status,
                    message=message
                });
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "DuplicateEmail":
                            errorMessasge.Add("Địa chỉ email đã được đăng ký.");
                            break;

                        case "DuplicateUserName":
                            errorMessasge.Add("Tên tài khoản đã tồn tại.");
                            break;
                    }
                }
                return Json(new
                {
                    message = errorMessasge,
                    status = false
                });
            }
        }
        public IActionResult ConfirmEmail(string userid, string token)
        {
            var message = string.Empty;
            var user = _userManager.FindByIdAsync(userid).Result;
            IdentityResult result = _userManager.
                        ConfirmEmailAsync(user, token).Result;
            if (result.Succeeded)
            {
                ViewBag.Message = "Xác thực email thành công!";
                return Redirect("/login");
            }
            else
            {
                ViewBag.Message = "Lỗi xác thực email!";
                return View("Error");
            }
        }
        [Authorize]
        [Route("/logout/{id}")]
        public async Task<IActionResult> LogOut(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user.Id!=null)
            {

                await _signInManager.SignOutAsync();
                return Redirect("/");
            }
            else
            {
                return Redirect("/");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> uploadAvatar(string image, string userid)
        {

            var uploadStatus = false;
            if (image != null)
            {
                var user = await _userManager.FindByIdAsync(userid);
                user.UrlAvatar = image;
                await _userManager.UpdateAsync(user);
                uploadStatus = true;
            }
            return Json(new
            {
                status = uploadStatus
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(string user)
        {
            var data = JsonConvert.DeserializeObject<ApplicationUser>(user);
            var userTarget = await _userManager.FindByIdAsync(data.Id);
            userTarget.DisplayName = data.DisplayName;
            userTarget.Address = data.Address;
            userTarget.PhoneNumber = data.PhoneNumber;
            await _userManager.UpdateAsync(userTarget);
            return Json(new
            {
                status=true,
                message=ResultState.Update_SUCCESS
            });
        }
    }
}