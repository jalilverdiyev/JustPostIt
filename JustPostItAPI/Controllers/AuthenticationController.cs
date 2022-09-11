using JustPostItAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace JustPostItAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{

    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ILogger<AuthenticationController> logger)
    {
        _logger = logger;
    }

    [HttpPost("Login")]
    public Person Login(User user)
    {
        return DbController.Authenticate(user);
    }
   
    [HttpPost("Register")]
    public bool Register(User user)
    {
        return DbController.Add(user);
    }
   
    
}