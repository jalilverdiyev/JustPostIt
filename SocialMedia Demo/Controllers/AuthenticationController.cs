using Microsoft.AspNetCore.Mvc;
using SocialMedia_Demo.Models;

namespace SocialMedia_Demo.Controllers;

public class AuthenticationController : Controller
{
    // GET
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Register(User usr)
    {
        bool isSuccess =  DbController.Add(usr);
        ContentResult cr = new ContentResult();
        if (isSuccess)
        {
            cr.Content = "Added successfully";
            return cr;
        }

        cr.Content = "There was some errors";
        return cr;
    }
    [HttpPost]
    public IActionResult Login(User user)
    {
        bool isAuth = DbController.Authenticate(user);
        ContentResult cr = new ContentResult();
        cr.Content = isAuth ? "Success" : "Fail";
        return cr;
    }
}