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
        var posts = DbController.GetPosts(_id,DbController.GetPostType.People);
        ViewBag.posts = posts;
        return View();
    }

    public IActionResult FriendsPosts()
    {
        var friends = DbController.GetFriends(_id);
        List<List<Post>> posts = new List<List<Post>>();
        foreach (var friend in friends)
        {
            posts.Add(DbController.GetPosts(friend.PersonId,DbController.GetPostType.Self));
        }
        ViewBag.posts = posts;
        return View();
    }

    public IActionResult SelfPosts()
    {
        var posts = DbController.GetPosts(_id, DbController.GetPostType.Self);
        ViewBag.posts = posts;
        return View();
    }
    
    public IActionResult People()
    {
        
        List<Person> people = DbController.GetPeople(Convert.ToInt32(_id));
        ViewBag.people = people;
        return View();
    }

    public IActionResult Friends()
    {
        var friends = DbController.GetFriends(_id).FindAll(friend => friend.Status == PersonStatus.Accepted);
        ViewBag.friends = friends;
        return View();
    }

    public IActionResult FriendRequests()
    {
        var requests = DbController.GetFriendRequests(_id).FindAll(friend => friend.Status == PersonStatus.Pending);
        ViewBag.requests = requests;
        return View();
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