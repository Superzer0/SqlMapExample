using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SqlMapExample
{
    public class ExampleContext : DbContext
    {
        public ExampleContext(DbContextOptions<ExampleContext> options) : base(options)
        {

        }

        public DbSet<FilesHistory> History { get; set; }
    }

    public class FilesHistory
    {
        public int Id { get; set; }
        public string SlugId { get; set; }
        public long Version { get; set; }
    }

}
