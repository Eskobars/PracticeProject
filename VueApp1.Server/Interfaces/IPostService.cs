using VueApp1.Server.Models.Posts;
using VueApp1.Server.Models.DTOs;

public interface IPostService
{
    Task<List<Post>> GetPostsAsync();
    Task<Post> GetPostByIdAsync(int id);
    Task<Post> CreatePostAsync(Post post);
    Task<Post> UpdatePostAsync(int id, Post post);
    Task<Post> PatchPostAsync(int id, PostPatchDto patchData);
    Task<bool> DeletePostAsync(int id);
}