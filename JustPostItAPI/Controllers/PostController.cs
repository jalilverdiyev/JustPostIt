using JustPostItAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JustPostItAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("GetAllPosts")]
    public List<Post> GetAllPosts(int id)
    {
        return DbController.GetPosts(id, DbController.GetPostType.People);
    }

    [HttpGet("GetFriendsPosts")]
    public List<List<Post>> GetFriendsPosts(int id)
    {
        var friends = DbController.GetFriends(id).FindAll(friend => friend.Status == PersonStatus.Accepted);
        List<List<Post>> posts = new List<List<Post>>();
        foreach (var friend in friends)
        {
            posts.Add(DbController.GetPosts(friend.PersonId,DbController.GetPostType.Self));
        }

        return posts;
    }

    [HttpGet("GetSelfPosts")]
    public List<Post> GetSelfPosts(int id)
    {
        return DbController.GetPosts(id, DbController.GetPostType.Self);
    }

    [HttpPost("AddNewPost")]
    public bool AddNewPost([FromForm]string post)
    {
        var poste = JsonConvert.DeserializeObject<Post>(post);
        return DbController.Add(poste);
    }

    [HttpGet("GetFriendPosts")]
    public List<Post> GetFriendPosts(int id)
    {
        return DbController.GetPosts(id, DbController.GetPostType.Self);
    }
}