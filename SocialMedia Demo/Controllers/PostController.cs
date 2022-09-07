using Microsoft.AspNetCore.Mvc;
using SocialMedia_Demo.Models;

namespace SocialMedia_Demo.Controllers;

public class PostController : Controller
{
    private readonly int _id;
    public PostController(IHttpContextAccessor httpContextAccessor)
    {
        _id = Convert.ToInt32(httpContextAccessor.HttpContext!.User.Claims.First(c => c.Type.Contains("nameidentifier")).Value);
    }
    [HttpPost]
    
    public IActionResult NewPost(Post post, string orders)
    {
        post.OwnerId = _id;
        post.Orders = orders.Split(',').Select(int.Parse).ToList();
        if(DbController.Add(post))
            return RedirectToAction("Index", "Home");
        return BadRequest();
    }

    [HttpPost]
    public IActionResult GetFriendPosts(int id)
    {
        var posts = DbController.GetPosts(id, DbController.GetPostType.Self);
        ViewBag.posts = posts;
        var view = View();
        return view;
    }

}