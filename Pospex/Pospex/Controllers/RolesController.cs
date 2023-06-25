using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pospex.Models;
using Pospex.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Pospex.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Edit(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {   
            User user = await _userManager.FindByIdAsync(userId);

            User currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var addedRoles = roles.Except(userRoles);

                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                if (user == currentUser)
                {
                    await _signInManager.RefreshSignInAsync(user);
                }

                return RedirectToAction("Index", "Users");
            }

            return NotFound();
        }
    }
}
