using EMSwebapp.Models;
using EMSwebapp.VIewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMSwebapp.Controllers
{
    public class AccountController : Controller
    {

        // Configures REG PAGE


        //MANAGE USER ACTIVITIES LIKE CRUD  
        public UserManager<ApplicationUser> _userManager { get; }

        //LOG IN USER DETAILS
        public SignInManager<ApplicationUser> _signInManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager, 
                                  SignInManager<ApplicationUser> signInManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel userViewModel)
        {
            if(ModelState.IsValid)
            {
                var userModel = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    

                };
               var result = await _userManager.CreateAsync(userModel, userViewModel.Password);
                if (result.Succeeded)
                {
                    //LOG IN THE USER AUTOMATICALLY
                   await _signInManager.SignInAsync(userModel, isPersistent: false);
                   return RedirectToAction("Home", "Index");
                }
            }

            return View(userViewModel);
        }


    }
}
