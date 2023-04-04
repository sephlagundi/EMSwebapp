﻿using EMSwebapp.Models;
using EMSwebapp.VIewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;

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
                   return RedirectToAction("Index", "Home");
                }

                foreach(var error in result.Errors) 
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            return View(userViewModel);
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInUserViewModel userViewModel)
        {
            if(ModelState.IsValid)
            {
                //LOGIN ACTIVITY -> COOKIE [Roles and Claims]
                var result = await _signInManager.PasswordSignInAsync(userViewModel.UserName, userViewModel.Password, userViewModel.RememberMe, false);
                //COOKIE WILL BE CREATED and TRANSFER TO THE CLIENT
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid LogIn Credentials");

            }

            return View(userViewModel);
        }

        [HttpGet]
        public async Task <IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LogIn");
        }






        }
}
