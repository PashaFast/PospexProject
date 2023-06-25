using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pospex.Models;
using Pospex.ViewModels;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pospex.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;


        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {

                User currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
                var userRoles = await _userManager.GetRolesAsync(currentUser);

                ViewBag.UserRoles = userRoles;
            }
            return View(_userManager.Users.ToList());
        }


        [Authorize]
        public IActionResult Create() => View();

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            User currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user != null && currentUser != null)
            {
                if (currentUser != user)
                {
                    IdentityResult result = await _userManager.DeleteAsync(user);
                    return RedirectToAction("Index");
                }
                else
                {
                    var userRoles = await _userManager.GetRolesAsync(currentUser);
                    ViewBag.UserRoles = userRoles;
                    ViewBag.Message = "Error: Can not delete current user!";
                    return View("Index", _userManager.Users.ToList());
                }

            }
            else
            {
                var userRoles = await _userManager.GetRolesAsync(currentUser);
                ViewBag.UserRoles = userRoles;
                ViewBag.Message = "Error: User is not found!";
                return View("Index", _userManager.Users.ToList());
            }
        }
    }
}
