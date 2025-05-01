using Microsoft.AspNetCore.Mvc;
using VueApp1.Server.Models.Posts;
using VueApp1.Server.Models.DTOs;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ILogger<PostsController> _logger;

    public PostsController(IPostService postService, ILogger<PostsController> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all posts.
    /// </summary>
    /// <returns>A list of posts.</returns>
    [HttpGet]
    public async Task<ActionResult<List<Post>>> GetPosts()
    {
        try
        {
            var posts = await _postService.GetPostsAsync();

            if (posts == null || posts.Count == 0)
            {
                return NotFound("No posts found.");
            }

            return Ok(posts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching posts.");
            return StatusCode(500, "An error occurred while retrieving posts.");
        }
    }

    /// <summary>
    /// Retrieves a specific post by ID.
    /// </summary>
    /// <param name="id">The ID of the post.</param>
    /// <returns>The post with the specified ID.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPostById(int id)
    {
        try
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound($"Post with ID {id} not found.");
            }

            return Ok(post);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching post with ID {id}.");
            return StatusCode(500, "An error occurred while retrieving the post.");
        }
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="post">The post to create.</param>
    /// <returns>The created post.</returns>
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPost = await _postService.CreatePostAsync(post);
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating post.");
            return StatusCode(500, "An error occurred while creating the post.");
        }
    }

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="id">The ID of the post to update.</param>
    /// <param name="post">The updated post data.</param>
    /// <returns>The updated post.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<Post>> UpdatePost(int id, [FromBody] Post post)
    {
        try
        {
            if (id != post.Id)
            {
                return BadRequest("Post ID mismatch.");
            }

            var updatedPost = await _postService.UpdatePostAsync(id, post);

            if (updatedPost == null)
            {
                return NotFound($"Post with ID {id} not found.");
            }

            return Ok(updatedPost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating post with ID {id}.");
            return StatusCode(500, "An error occurred while updating the post.");
        }
    }

    /// <summary>
    /// Patches an existing post.
    /// </summary>
    /// <param name="id">The ID of the post to patch.</param>
    /// <param name="patchDto">The data for the patch update.</param>
    /// <returns>The patched post.</returns>
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchPost(int id, [FromBody] PostPatchDto patchDto)
    {
        try
        {
            var patchedPost = await _postService.PatchPostAsync(id, patchDto);
            return patchedPost != null ? Ok(patchedPost) : NotFound($"Post with ID {id} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error patching post with ID {id}.");
            return StatusCode(500, "An error occurred while patching the post.");
        }
    }

    /// <summary>
    /// Deletes a post by ID.
    /// </summary>
    /// <param name="id">The ID of the post to delete.</param>
    /// <returns>No content if successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            var success = await _postService.DeletePostAsync(id);

            if (!success)
            {
                return NotFound($"Post with ID {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting post with ID {id}.");
            return StatusCode(500, "An error occurred while deleting the post.");
        }
    }
}
