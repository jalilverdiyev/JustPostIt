using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Demo.Models;

namespace SocialMedia_Demo.Controllers;

[Authorize]
public class PostController : Controller
{
    private readonly int _id;
    private readonly HttpClientHandler _handler = new HttpClientHandler(); 
    public PostController(IHttpContextAccessor httpContextAccessor)
    {
        _id = Convert.ToInt32(httpContextAccessor.HttpContext!.User.Claims.First(c => c.Type.Contains("nameidentifier")).Value);
        _handler.UseDefaultCredentials = true;
        _handler.ServerCertificateCustomValidationCallback = (_,_,_,_) => true;
    }
    
    public async Task<IActionResult> AllPosts()
    {
        string requestUrl = $"https://localhost:7162/api/Post/GetAllPosts?id={_id}";
        HttpClient client = new HttpClient(_handler);
        var response = await client.GetStringAsync(requestUrl);
        var posts = JsonConvert.DeserializeObject<List<Post>>(response);
        ViewBag.posts = posts ?? new List<Post>();
        return View();
    }

    public async Task<IActionResult> FriendsPosts()
    {
        string requestUrl = $"https://localhost:7162/api/Post/GetFriendsPosts?id={_id}";
        HttpClient client = new HttpClient(_handler);
        var response = await client.GetStringAsync(requestUrl);
        var posts = JsonConvert.DeserializeObject<List<List<Post>>>(response);
        ViewBag.posts = posts ?? new List<List<Post>>();
        return View();
    }

    public async Task<IActionResult> SelfPosts()
    {
        string requestUrl = $"https://localhost:7162/api/Post/GetSelfPosts?id={_id}";
        HttpClient client = new HttpClient(_handler);
        var response = await client.GetStringAsync(requestUrl);
        var posts = JsonConvert.DeserializeObject<List<Post>>(response);
        ViewBag.posts = posts ?? new List<Post>();
        return View();
    }
    
    
    [HttpPost]
    // public async Task<IActionResult> NewPost(Post post, string orders)
    // {
    //     post.Orders = orders.Split(',').Select(int.Parse).ToList();
    //     post.OwnerId = _id;
    //     post.Photos = Request.Form.Files as FormFile[];
    //     string requestUrl = "https://localhost:7162/api/Post/AddNewPost";
    //     HttpClient client = new HttpClient(_handler);
    //     var response = await client.PostAsJsonAsync(requestUrl,post);
    //     
    //     return RedirectToAction("AllPosts", "Post");
    //     return BadRequest();
    // }

    [HttpPost]
    public async Task<IActionResult> GetFriendPosts(int id)
    {
        string requestUrl = $"https://localhost:7162/api/Post/GetFriendPosts?id={id}";
        HttpClient client = new HttpClient(_handler);
        var response = await client.GetStringAsync(requestUrl);
        var posts = JsonConvert.DeserializeObject<List<Post>>(response);
        ViewBag.posts = posts ?? new List<Post>();
        var view = View();
        return view;
    }

}