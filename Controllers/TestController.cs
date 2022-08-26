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
        private readonly string _protectionLevel;

        public TestController(ILogger<TestController> logger, ExampleContext exampleContext, IConfiguration configuration)
        {
            _logger = logger;
            _exampleContext = exampleContext;
            _protectionLevel = configuration["PROTECTION_LEVEL"];
        }

        [HttpGet(nameof(GetId))]
        public async Task<IActionResult> GetId(string slugId)
        {
            _logger.LogInformation("Running with {protectionLevel} protection mode", _protectionLevel);

            try
            {
                var history = await GetDataWithProtectionLevel(slugId, _protectionLevel);
                return Ok(history);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Got some error");
                return NotFound();
            }
        }

        private async Task<List<FilesHistory>> GetDataWithProtectionLevel(string slugId, string protectionLevel) =>
            protectionLevel switch
            {
                "NONE" => await FilesHistoriesNoProtection(slugId),
                "INTEGRATED" => await FilesHistoriesIntegrated(slugId),
                "MANUAL" => await FilesHistoriesManual(slugId),
                _ => throw new ArgumentOutOfRangeException(nameof(protectionLevel), protectionLevel, null)
            };

        private async Task<List<FilesHistory>> FilesHistoriesManual(string slugId)
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
                        [SlugId] = '{slugId}'";

            return await _exampleContext.History.FromSqlRaw(sql).AsNoTracking().ToListAsync();
        }

        private async Task<List<FilesHistory>> FilesHistoriesNoProtection(string slugId)
        {
            var sql = @$"
                    SELECT
                       ID,
                       SlugId,
                       Version
                    FROM
                        [History]
                    WHERE
                        [SlugId] = '{slugId}'
                    ";

            return await _exampleContext.History.FromSqlRaw(sql).AsNoTracking().ToListAsync();
        }

        private async Task<List<FilesHistory>> FilesHistoriesIntegrated(string slugId)
        {
            FormattableString sql = @$"
                    SELECT
                       ID,
                       SlugId,
                       Version
                    FROM
                        [History]
                    WHERE
                        [SlugId] = '{slugId}'
                    ";

            return await _exampleContext.History.FromSqlInterpolated(sql).AsNoTracking().ToListAsync();
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