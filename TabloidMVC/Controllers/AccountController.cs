using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public AccountController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            var userProfile = _userProfileRepository.GetByEmail(credentials.Email);

            if (userProfile == null || userProfile.ActiveFlag==false)
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserProfile userProfile)
        {
            _userProfileRepository.Add(userProfile);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Index() 
        {
          
            List<UserProfile> allUsers = _userProfileRepository.GetAll();
            return View(allUsers);
        
        }

        public IActionResult Details(int Id)
        {
            UserProfile chosenOne = _userProfileRepository.GetById(Id);
            return View(chosenOne);
        }

        public IActionResult Deactivate(int id) 
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);
           
            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deactivate(UserProfile deactivateUser)
        {
            
            _userProfileRepository.DeactivateById(deactivateUser.Id);
            return RedirectToAction("Index"); ;
        }

        public IActionResult Edit(int id) 
        {
            
        }

        public IActionResult Edit(int id, UserProfile target) 
        {
        
        }
    }
}
