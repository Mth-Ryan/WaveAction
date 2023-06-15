using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WaveAction.Data;
using WaveAction.Dtos.Threads;

namespace WaveAction.Controllers;

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

    [HttpGet("{id}", Name = "Threads Get")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new ThreadDto());
    }

    [HttpGet("{name}", Name = "Threads Get From Name")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(string name)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new ThreadDto());
    }


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