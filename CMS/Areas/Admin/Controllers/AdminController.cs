using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using CMS.Core.Services.Interfaces;
using CMS.Core.DTOs;
using CMS.DataLayer.Context;
using CMS.DataLayer.Models;
using CMS.Core.Convertors;
using Microsoft.Extensions.Hosting;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private IAdminService _adminService;
        private IUserService _userService;
        private IPostService _postService;

        public AdminController(IAdminService adminService, IUserService userService, IPostService postService)
        {
            _adminService = adminService;
            _userService = userService;
            _postService = postService;
        }

        #region Users

        public IActionResult Users()
        {
            return View(_userService.GetAllUsers());
        }

        public IActionResult EditUser(int Id)
        {
            return View(_userService.GetUserById(Id));
        }

        [HttpPost]
        public IActionResult EditUser(User user, IFormFile img)
        {
            if (user != null && img != null)
            {
                #region UpdatePic

                string recentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", user.ImagePath);
                System.IO.File.Delete(recentPath);

                using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PostsPic", img.FileName), FileMode.Create))
                {
                    img.CopyTo(stream);
                }

                #endregion

                user.ImagePath = img.FileName;
                _userService.UpdateUser(user);
                return Redirect("/Admin");
            }
            else if (user != null)
            {
                _userService.UpdateUser(user);
                return Redirect("/Admin");
            }

            return BadRequest();
        }

        #endregion

        #region Posts

        public IActionResult Posts()
        {
            return View(_postService.GetAllPosts());
        }

        public IActionResult EditPost(int postId)
        {
            return View(_postService.GetPostById(postId));
        }

        [HttpPost]
        public IActionResult EditPost(Post post, IFormFile img)
        {
            if (post != null && img != null)
            {
                #region UpdatePic

                string recentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PostsPic", post.ImagePath);
                System.IO.File.Delete(recentPath);

                using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PostsPic", img.FileName), FileMode.Create))
                {
                    img.CopyTo(stream);
                }

                #endregion

                post.UpdateDate = DateTime.Now.ToShamsi();
                post.ImagePath = img.FileName;
                _postService.UpdatePost(post);
                return Redirect("/Admin");
            }
            else if (post != null)
            {
                post.UpdateDate = DateTime.Now.ToShamsi();
                _postService.UpdatePost(post);
                return Redirect("/Admin");
            }

            return View(post);
        }

        #endregion

        #region Comments

        public IActionResult Comments()
        {
            return View(_postService.GetAllComments());
        }

        public IActionResult DeleteComment(int commentId)
        {
            if (commentId != 0)
            {
                _postService.DeleteComment(commentId);
                return Redirect("/Admin");
            }

            return BadRequest();
        }

        #endregion

        #region Reactions

        public IActionResult Reactions()
        {
            return View(_postService.GetAllReactions());
        }

        #endregion

        #region Categories

        public IActionResult Categories()
        {
            return View(_postService.GetAllCategories());
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name))
            {
                _postService.AddCategory(category);
                return Redirect("/Admin");
            }

            return BadRequest();
        }

        #endregion
    }
}