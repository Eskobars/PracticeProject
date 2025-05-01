using VueApp1.Server.Models.Posts;
using VueApp1.Server.Models.DTOs;

public class PostService : IPostService
{
    private readonly HttpClient _httpClient;

    public PostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    }

    public async Task<List<Post>> GetPostsAsync()
    {
        var response = await _httpClient.GetAsync("posts");

        response.EnsureSuccessStatusCode();

        var posts = await response.Content.ReadFromJsonAsync<List<Post>>();
        return posts!;
    }

    public async Task<Post> GetPostByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"posts/{id}");

        response.EnsureSuccessStatusCode();

        var post = await response.Content.ReadFromJsonAsync<Post>();
        return post!;
    }

    public async Task<Post> CreatePostAsync(Post post)
    {
        var content = JsonContent.Create(post);
        var response = await _httpClient.PostAsync("posts", content);

        response.EnsureSuccessStatusCode();

        var createdPost = await response.Content.ReadFromJsonAsync<Post>();
        return createdPost!;
    }

    public async Task<Post> UpdatePostAsync(int id, Post post)
    {
        var content = JsonContent.Create(post);
        var response = await _httpClient.PutAsync($"posts/{id}", content);
        response.EnsureSuccessStatusCode();

        var updatedPost = await response.Content.ReadFromJsonAsync<Post>();
        if (updatedPost == null)
        {
            throw new InvalidOperationException("Failed to deserialize the response to a Post object.");
        }

        return updatedPost;
    }
    public async Task<Post> PatchPostAsync(int id, PostPatchDto patchDto)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"posts/{id}")
        {
            Content = JsonContent.Create(patchDto)
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var patchedPost = await response.Content.ReadFromJsonAsync<Post>();
        if (patchedPost == null)
        {
            throw new InvalidOperationException("Failed to deserialize the patched post.");
        }

        return patchedPost;
    }


    public async Task<bool> DeletePostAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"posts/{id}");
        return response.IsSuccessStatusCode;
    }
}
