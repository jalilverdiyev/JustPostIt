using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SocialMedia_Demo.Models;

namespace SocialMedia_Demo.Controllers;

public class AuthenticationController : Controller
{
    // GET
    [HttpGet("login")]
    public IActionResult Login(string returnUrl)
    {
        ViewData["returnUrl"] = returnUrl;
        return View();
    }
    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(User user,  string rememberMe,string returnUrl="/")
    {
        var authed = DbController.Authenticate(user);
        if (authed.Item2)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("username",authed.Item1.Name));
            claims.Add(new Claim("profile",authed.Item1.Profile_Photo)); 
            claims.Add(new Claim(ClaimTypes.NameIdentifier, authed.Item1.PersonId.ToString()));
            var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            bool isPersist = rememberMe == "on";
            await HttpContext.SignInAsync(principal,new AuthenticationProperties
            {
                IsPersistent = isPersist
            });
            return Redirect(returnUrl);
        }

        return BadRequest();
    }
    
    [HttpPost("register")]
    public IActionResult Register(User usr)
    {
        bool isSuccess =  DbController.Add(usr);
        ContentResult cr = new ContentResult();
        if (isSuccess)
        {
            return Redirect("login");
        }

        cr.Content = "There was some errors";
        return cr;
        //Todo:  Make better error logs or something
    }
    
}