using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SqlMapExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly ExampleContext _exampleContext;

        public TestController(ILogger<TestController> logger, ExampleContext exampleContext)
        {
            _logger = logger;
            _exampleContext = exampleContext;
        }

        [HttpGet(nameof(GetId))]
        public async Task<IActionResult> GetId(string slugId, long version)
        {
            try
            {
                if (slugId.Any(@char => !(char.IsLetterOrDigit(@char) || @char == '-')))
                {
                    throw new Exception("Hacking not allowed.");
                }

                var sql = @$"
                    SELECT
                       ID,
                       SlugId,
                       Version
                    FROM
                        [History]
                    WHERE
                        [SlugId] = '{slugId}'
                    AND
                        [Version] = '{version}'
                    ";

                var history = await _exampleContext.History.FromSqlRaw(sql).AsNoTracking().ToListAsync();
                return Ok(history);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Got some error");
                return NotFound();
            }
        }

        [HttpGet(nameof(EnsureDataSeed))]
        public async Task<IActionResult> EnsureDataSeed()
        {
            if (await _exampleContext.History.AnyAsync())
            {
                return Ok("Seed already in the db");
            }

            _exampleContext.History.AddRange(new List<FilesHistory>
            {
                new()
                {
                    SlugId = "aaa",
                    Version = 1
                },
                new()
                {
                    SlugId = "bbb",
                    Version = 2
                },
                new()
                {
                    SlugId = "ccc",
                    Version = 3
                },
            });

            await _exampleContext.SaveChangesAsync();
            return Ok("Seed added");
        }
    }
}