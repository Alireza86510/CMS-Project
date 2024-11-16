using CMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CMS.Core.DTOs;

namespace CMS.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;
        private IPostService _postService;

        public HomeController(IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;
        }

        public IActionResult Index()
        {
            return View(_postService.GetAllPosts());
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Search(string search)
        {
            return View("Index", _postService.GetPostsByFilter(search));
        }
    }
}
