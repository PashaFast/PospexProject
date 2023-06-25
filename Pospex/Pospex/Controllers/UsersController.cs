using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pospex.Models;
using Pospex.ViewModels;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                ViewBag.CurrentUser = currentUser;

            }
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                user.Role = rolesList[0];
            }

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.Message = TempData["ErrorMessage"].ToString();
            }
            return View(users);
        }


        [Authorize]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
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
                    TempData["ErrorMessage"] = "Error: Can not delete current user!";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Error: User is not found!";
                return RedirectToAction("Index");

            }
        }


        [Authorize]
        public IActionResult AddAvatar() => View();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAvatar(AddAvatarViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(User.Identity.Name);

                if (user != null)
                {

                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        byte[] avatar = null;
                        using (var fileStream = files[0].OpenReadStream())
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                fileStream.CopyTo(memoryStream);
                                avatar = memoryStream.ToArray();

                            }
                        }

                        user.Avatar = avatar;
                    }
                    else
                    {
                        ViewBag.Message = "Error: No file selected!";
                        return View(model);
                    }
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
    }
}
