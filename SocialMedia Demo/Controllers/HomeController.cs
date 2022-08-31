using Microsoft.AspNetCore.Mvc;
using SocialMedia_Demo.Models;

namespace SocialMedia_Demo.Controllers;

public class HomeController : Controller
{

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
        List<Person> people = DbController.GetUsers(2);
        
        return View(people);
    }

    public IActionResult Friends()
    {
        var list = DbController.GetFriends(2).FindAll(friend => friend.Status == PersonStatus.Accepted);
        return View(list);
    }

    public IActionResult FriendRequests()
    {
        var friends = DbController.GetFriendRequests(2).FindAll(friend => friend.Status == PersonStatus.Pending);
        return View(friends);
    }
    
    [HttpPost]
    public IActionResult AddFriend(int id,Person person)
    {
        bool success = DbController.AddFriend(id, person);
        ViewBag.added = success;
        var view = View();
        return view;
        
    }

    [HttpPost]
    public IActionResult UpdateFriend(int id, Person person)
    {
        bool succeed = DbController.UpdateFriend(id, person);
        ViewBag.succeed = succeed;
        var view = View();
        return view;
    }
}