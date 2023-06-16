using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaveActionApi.Data;
using WaveActionApi.Dtos.Threads;

namespace WaveActionApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ThreadsController : ControllerBase
{
    private readonly ILogger<ThreadsController> _logger;
    private readonly BlogContext _blogContext;
    private readonly IMapper _mapper;

    public ThreadsController(ILogger<ThreadsController> logger, BlogContext blogContext, IMapper mapper)
    {
        _logger = logger;
        _blogContext = blogContext;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Threads Get")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new ThreadDto());
    }

    [AllowAnonymous]
    [HttpGet("{name}", Name = "Threads Get From Name")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(string name)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new ThreadDto());
    }


    [AllowAnonymous]
    [HttpGet(Name = "Threads Get All")]
    [ProducesResponseType(typeof(List<ThreadDto>), 200)]
    public async Task<IActionResult> Get(uint PageSize, uint Page)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new List<ThreadDto>());
    }

    [HttpPost(Name = "Threads Create")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Create([FromBody] ThreadCreateDto threadCreate)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new ThreadDto());
    }

    [HttpPut("{id}", Name = "Threads Update")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ThreadCreateDto threadCreate)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new ThreadDto());
    }

    [HttpDelete("{id}", Name = "Threads Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok();
    }
}