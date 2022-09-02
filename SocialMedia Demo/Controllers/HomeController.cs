using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia_Demo.Models;

namespace SocialMedia_Demo.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly int _id;
    public HomeController(IHttpContextAccessor httpContextAccessor)
    {
        _id = Convert.ToInt32(httpContextAccessor.HttpContext!.User.Claims.First(c => c.Type.Contains("nameidentifier")).Value);
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult FriendsPosts()
    {
        return View();
    }

    public IActionResult People()
    {
        
        List<Person> people = DbController.GetPeople(Convert.ToInt32(_id));
        
        return View(people);
    }

    public IActionResult Friends()
    {
        var list = DbController.GetFriends(_id).FindAll(friend => friend.Status == PersonStatus.Accepted);
        return View(list);
    }

    public IActionResult FriendRequests()
    {
        var friends = DbController.GetFriendRequests(_id).FindAll(friend => friend.Status == PersonStatus.Pending);
        return View(friends);
    }
    
    [HttpPost]
    public IActionResult AddFriend(Person person)
    {
        bool success = DbController.AddFriend(_id, person);
        ViewBag.added = success;
        var view = View();
        return view;
        
    }

    [HttpPost]
    public IActionResult UpdateFriend( Person person)
    {
        bool succeed = DbController.UpdateFriend(_id, person);
        ViewBag.succeed = succeed;
        var view = View();
        return view;
    }
}