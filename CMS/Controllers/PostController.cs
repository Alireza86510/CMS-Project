using CMS.Core.DTOs;
using CMS.Core.Services.Interfaces;
using CMS.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class PostController : Controller
    {
        private IPostService _postService;
        private IUserService _userService;

        public PostController(IPostService postService, IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        public IActionResult ShowPost(int postId)
        {
            var post = _postService.GetPostById(postId);
            if (post != null)
            {
                ViewBag.PostReactions = _postService.GetPostReactionsByPostId(postId);
                return View(post);
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult ShowCategory(int categoryId)
        {
            return View(_postService.GetPostByCategory(categoryId));
        }

        public ActionResult AddPost()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }

        [HttpPost]
        public IActionResult AddPost(AddPostViewModel post, int categoryId, IFormFile img)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!TryValidateModel(post))
                {
                    return View(post);
                }

                #region Upload Image

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PostsPic", img.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    img.CopyTo(stream);
                }

                #endregion

                Post finalPost = new Post
                {
                    UserId = _userService.GetUserIdByEmail(User.Identity.Name),
                    CategoryId = categoryId,
                    Content = post.Content,
                    CreateDate = DateTime.Parse("4/3/2020 12:00:00 AM"),
                    ImagePath = img.FileName,
                    Tags = post.Tags,
                    Title = post.Title,
                    UpdateDate = DateTime.Parse("4/3/2020 12:00:00 AM")
                };

                if (_postService.AddPost(finalPost))
                {
                    return View("Success");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }

        [HttpPost]
        public IActionResult Comment(int postId, string message)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (postId != null && !string.IsNullOrEmpty(message))
                {
                    _postService.Comment(_userService.GetUserIdByEmail(User.Identity.Name), postId, message);
                    ViewBag.PostReactions = _postService.GetPostReactionsByPostId(postId);
                    return View("ShowPost", _postService.GetPostById(postId));
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return Redirect("/Account/Login");
            }
        }

        public IActionResult Like(int postId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (_postService.Like(_userService.GetUserIdByEmail(User.Identity.Name), postId))
            {
                ViewBag.PostReactions = _postService.GetPostReactionsByPostId(postId);
                return View("ShowPost", _postService.GetPostById(postId));
            }

            return BadRequest();
        }
    }
}
