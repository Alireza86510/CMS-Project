using System.Security.Claims;
using CMS.Core.Convertors;
using CMS.Core.DTOs;
using CMS.Core.Generators;
using CMS.Core.Security;
using CMS.Core.Senders;
using CMS.Core.Services.Implements;
using CMS.Core.Services.Interfaces;
using CMS.DataLayer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace CMS.Controllers;

public class AccountController : Controller
{
    private IUserService _userService;
    private IPostService _postService;

    public AccountController(IUserService userService, IPostService postService)
    {
        _userService = userService;
        _postService = postService;
    }

    public IActionResult Login()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return View();
        }
        else
        {
            return Redirect("/");
        }
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel login)
    {
        if (!ModelState.IsValid)
        {
            return View(login);
        }

        var user = _userService.GetUserForLogin(login);

        if (user == null)
        {
            ModelState.AddModelError("Email", "کاربری با اطلاعات وارد شده یافت نشد");
            return View(login);
        }

        #region Claims

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email.ToLower())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties
        {
            IsPersistent = login.RememberMe,
        };
        HttpContext.SignInAsync(principal, properties);

        #endregion

        return View("SuccessLogin");
    }

    public IActionResult Register()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return View();
        }
        else
        {
            return Redirect("/");
        }
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel register)
    {
        if (!ModelState.IsValid)
        {
            return View(register);
        }

        if (!_userService.CheckUserExistByEmail(register.Email.ToLower()))
        {
            DataLayer.Models.User user = new User
            {
                Name = register.Name,
                Email = register.Email.ToLower(),
                Password = PasswordHash.Hash(register.Password),
                ActiveCode = NameGenerator.GenerateCode(),
                RegisterDate = DateTime.Now.ToShamsi(),
                IsActive = false,
                IsAdmin = false
            };
            _userService.AddUser(user);
            EmailSender.SendEmail(register.Email, "فعالسازی حساب کاربری خبرگزاری NJR",
                "<p>کاربر عزیز، به وبسایت ما خوش آمدید!</p><br><p>جهت فعالسازی حساب کاربری خود روی لینک زیر کلیک کنید</p><br>" +
                "<a href=" + "https://localhost:7121/Account/Activation?activeCode=" + user.ActiveCode +
                ">فعالسازی حساب<a/>");
        }

        return View("SuccessRegister");
    }

    public IActionResult Activation(string activeCode)
    {
        if (_userService.ActiveAccount(activeCode))
        {
            return View("SuccessActivation");
        }
        else
        {
            return View("ErrorActivation");
        }
    }

    public IActionResult Page(int userId)
    {
        return View(_userService.GetUserForShow(userId));
    }

    public IActionResult EditProfile()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = _userService.GetUserByEmail(User.Identity.Name);
            return View(user);
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    [HttpPost]
    public IActionResult EditProfile(User user, IFormFile img)
    {
        if (user == null)
        {
            return View(user);
        }

        if (img != null)
        {
            #region Update Image

            string recentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", user.ImagePath);
            System.IO.File.Delete(recentPath);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", img.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                img.CopyTo(stream);
            }

            #endregion

            user.ImagePath = img.FileName;
        }

        if (_userService.UpdateUser(user))
        {
            return View("SuccessEdit");
        }
        else
        {
            return View("ErrorEdit");
        }
    }

    public IActionResult SendActivationEmail()
    {
        if (User.Identity.IsAuthenticated)
        {
            string activeCode = _userService.GetActiveCodeByEmail(User.Identity.Name);
            EmailSender.SendEmail(User.Identity.Name, "فعالسازی حساب کاربری خبرگزاری NJR",
                "<p>کاربر عزیز، به وبسایت ما خوش آمدید!</p><br><p>جهت فعالسازی حساب کاربری خود روی لینک زیر کلیک کنید</p><br>" +
                "<a href=" + "https://localhost:7121/Account/Activation?activeCode=" + activeCode +
                ">فعالسازی حساب<a/>");
            return View("SuccessSendEmail");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ForgotPassword(string email)
    {
        string activeCode = _userService.GetActiveCodeByEmail(email);
        if (activeCode != null)
        {
            EmailSender.SendEmail(email, "بازیابی رمز حساب کاربری خبرگزاری NJR",
                "<p>کاربر عزیز!</p><br><p>جهت بازیابی رمز حساب کاربری خود روی لینک زیر کلیک کنید</p><br>" +
                "<a href=" + "https://localhost:7121/Account/ResetPassword?activeCode=" + activeCode +
                ">فعالسازی حساب<a/>");
            return View("SuccessSendEmail");
        }
        else
        {
            return View("ErrorActivation");
        }
    }

    public IActionResult ResetPassword(string activeCode)
    {
        if (_userService.GetUserForResetPassword(activeCode) != null)
        {
            var user = _userService.GetUserForResetPassword(activeCode);
            return View(user);
        }
        else
        {
            return View("ErrorActivation");
        }
    }

    [HttpPost]
    public IActionResult ResetPassword(ResetPasswordViewModel resetPassword)
    {
        if (!ModelState.IsValid)
        {
            return View(resetPassword);
        }

        if (_userService.ResetPassword(resetPassword.ActiveCode, resetPassword.Password))
        {
            return View("SuccessActivation");
        }
        else
        {
            return View("ErrorActivation");
        }
    }

    public IActionResult Security()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login");
        }

        return View(new ChangePasswordViewModel());
    }

    [HttpPost]
    public IActionResult Security(ChangePasswordViewModel model)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.CurrentPassword != null && model.NewPassword != null && model.NewRePassword != null)
            {
                if (_userService.ChangePassword(model, User.Identity.Name))
                {
                    return View("SuccessActivation");
                }
                else
                {
                    return View("ErrorActivation");
                }
            }
        }
        else
        {
            return RedirectToAction("Login");
        }

        return BadRequest();
    }

    public IActionResult MyPosts()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login");
        }

        int authorId = _userService.GetUserIdByEmail(User.Identity.Name);
        return View(_postService.GetPostByAuthorId(authorId));
    }

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}