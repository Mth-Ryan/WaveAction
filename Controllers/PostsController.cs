using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WaveAction.Data;
using WaveAction.Dtos.Posts;

namespace WaveAction.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly ILogger<PostsController> _logger;
    private readonly BlogContext _blogContext;
    private readonly IMapper _mapper;

    public PostsController(ILogger<PostsController> logger, BlogContext blogContext, IMapper mapper)
    {
        _logger = logger;
        _blogContext = blogContext;
        _mapper = mapper;
    }

    [HttpGet("{id}", Name = "Posts Get")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new PostDto());
    }

    [HttpGet("{name}", Name = "Posts Get From Name")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Get(string name)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new PostDto());
    }


    [HttpGet(Name = "Posts Get All")]
    [ProducesResponseType(typeof(List<PostShortDto>), 200)]
    public async Task<IActionResult> Get(uint PageSize, uint Page)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new List<PostShortDto>());
    }

    [HttpPost(Name = "Posts Create")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Create([FromBody] PostCreateDto threadCreate)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new PostDto());
    }

    [HttpPut("{id}", Name = "Posts Update")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Update(Guid id, [FromBody] PostCreateDto threadCreate)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new PostDto());
    }

    [HttpDelete("{id}", Name = "Posts Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok();
    }
}