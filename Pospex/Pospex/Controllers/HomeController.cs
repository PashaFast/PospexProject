﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pospex.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pospex.Controllers
{
   //[Authorize]
   // public class HomeController : Controller
   // {
   //     private readonly ILogger<HomeController> _logger;
   //     UserManager<User> _userManager;

   //     public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
   //     {
   //         _logger = logger;
   //         _userManager = userManager;
   //     }

   //     public IActionResult Index()
   //     {
   //         return View(_userManager.Users.ToList());
   //     }

   //     public IActionResult Privacy()
   //     {
   //         return View();
   //     }

   //     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   //     public IActionResult Error()
   //     {
   //         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
   //     }
   // }
}
