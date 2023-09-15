using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Demo.Models;

namespace SocialMedia_Demo.Controllers;

[Authorize]
public class PersonController : Controller
{
    private readonly int _id;
    private HttpClientHandler _handler = new HttpClientHandler();
    public PersonController(IHttpContextAccessor httpContextAccessor)
    {
        _id = Convert.ToInt32(httpContextAccessor.HttpContext!.User.Claims.First(c => c.Type.Contains("nameidentifier")).Value);
        _handler.UseDefaultCredentials = true;
        _handler.ServerCertificateCustomValidationCallback = (_,_,_,_) => true;
    }
    
    public async Task<IActionResult> People()
    {
        string requestUrl = $"https://localhost:7162/api/Person/GetPeople?id={_id}";
        HttpClient client = new HttpClient(_handler);
        var response = await client.GetStringAsync(requestUrl);
        var people = JsonConvert.DeserializeObject<List<Person>>(response);
        ViewBag.people = people ?? new List<Person>();
        return View();
    }

    public async Task<IActionResult> Friends()
    {
        string requestUrl = $"https://localhost:7162/api/Person/GetFriends?id={_id}";
        HttpClient client = new HttpClient(_handler);
        var response = await client.GetStringAsync(requestUrl);
        var friends = JsonConvert.DeserializeObject<List<Person>>(response);
        ViewBag.friends = friends ?? new List<Person>();
        return View();
    }

    public async Task<IActionResult> FriendRequests()
    {
        string requestUrl = $"https://localhost:7162/api/Person/GetFriendsRequests?id={_id}";
        HttpClient client = new HttpClient(_handler);
        var response = await client.GetStringAsync(requestUrl);
        var requests = JsonConvert.DeserializeObject<List<Person>>(response);
        ViewBag.requests = requests ?? new List<Person>();
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddFriend(Person person)
    {
        string requestUrl = $"https://localhost:7162/api/Person/AddFriend?id={_id}";
        HttpClient client = new HttpClient(_handler);
        var response = await client.PostAsJsonAsync(requestUrl,person);
        bool success = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
        ViewBag.added = success;
        var view = View();
        return view;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateFriend( Person person)
    {
        string requestUrl = $"https://localhost:7162/api/Person/UpdateFriend?id={_id}";
        HttpClient client = new HttpClient(_handler);
        var response = await client.PutAsJsonAsync(requestUrl, person);
        bool succeed = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
        ViewBag.succeed = succeed;
        var view = View();
        return view;
    }
}