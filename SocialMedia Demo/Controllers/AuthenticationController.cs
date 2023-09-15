using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Demo.Models;

namespace SocialMedia_Demo.Controllers;

public class AuthenticationController : Controller
{
    private readonly HttpClientHandler _handler = new HttpClientHandler();

    public AuthenticationController()
    {
        _handler.UseDefaultCredentials = true;
        _handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;  
    }
    
    
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(User user, string rememberMe, string returnUrl = "/")
    {
        HttpClient client = new HttpClient(_handler);
        string requestUrl = "https://localhost:7162/api/Authentication/Login";
        var post = await client.PostAsJsonAsync(requestUrl, user);
        var result = post.Content.ReadAsStringAsync().Result;
        var authed = JsonConvert.DeserializeObject<Person>(result);
        
        if (authed is { IsValid: true })
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("username",authed.Name));
            claims.Add(new Claim("profile",authed.Profile_Photo)); 
            claims.Add(new Claim(ClaimTypes.NameIdentifier, authed.PersonId.ToString()));
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

    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

}