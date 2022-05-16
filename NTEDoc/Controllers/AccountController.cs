using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NTEDoc.Data;
using NTEDoc.DataRepository.Data;
using NTEDoc.DataRepository.Models;
using NTEDoc.Helpers.Auth;
using NTEDoc.Models;
using NTEDoc.Models.ViewModels;
using NTEDoc.Services;

namespace NTEDocSystemV2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EntityDbContext _entityContext;
        private readonly IUserService _userService;

        private readonly JwtTokenValidationSettings _jwtSettings;

        public AccountController(
            ApplicationDbContext context,
            EntityDbContext entityContext,
            IUserService userService, 
            IOptions<JwtTokenValidationSettings> jwtSettings)
        {
            _context = context;
            _entityContext = entityContext;
            _userService = userService;

            _jwtSettings = jwtSettings.Value;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                // var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                // Jwt auth
                var result = await _userService.Authenticate(model.UserName, model.Password);

                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, result.Username.ToString()),
                        new Claim(ClaimTypes.Role, result.RoleId.ToString())
                    }),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                HttpContext.Session.SetString("JWToken", tokenString);

                // Jwt auth end

                if (result != null)
                {
                    return RedirectToAction("Index", "Document");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            ViewData["SektorId"] = new SelectList(_entityContext.Sector, "Id", "Naziv");

            var dropList = _context.Roles.ToList();
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");

            return View();
        }

        /*[AllowAnonymous]
        [ValidateAntiForgeryToken]*/
        /*public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { FirstName = model.FirstName, LastName = model.LastName, UserName = model.Username, Email = model.Username, SektorId = model.SektorId };

                var result = await _
                
            
            ger.CreateAsync(user, model.Password);
                

                ApplicationUser user1 = await _userManager.FindByEmailAsync(model.Username);
                

                var role = await RoleManager.FindByIdAsync(model.RoleId);
                var roleName = role.Name;

                if(roleName == "Likvidator")
                {
                    Likvidatori likvidator = new Likvidatori() { FirstName = user1.FirstName, LastName = user1.LastName, UserId = user1.Id };
                    _entityContext.Likvidatori.Add(likvidator);
                    _entityContext.SaveChanges();
                }

                await _userManager.AddToRoleAsync(user1, roleName);

                if (result.Succeeded)
                {
                    // await _userManager.AddToRoleAsync(user, "Applicant");


                    // await _signInManager.SignInAsync(user, isPersistent: false);
                      return RedirectToAction("Index", "Document");
                    //return RedirectToAction(nameof(Login));
                    // return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(AccountController.Login));
        }


        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                //return Json(new { success = false, message = result.Errors.First().Description });
            }
        }
        #endregion
    }
}