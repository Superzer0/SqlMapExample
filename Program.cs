using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SqlMapExample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ExampleContext>(options =>
{
    var connectionString = new SqlConnectionStringBuilder
    {
        DataSource = "localhost",
        InitialCatalog = "TestDatabase",
        UserID = "sa",
        Password = "Password1!",
        MultipleActiveResultSets = true,
        Encrypt = false
    }.ConnectionString;

    options.UseSqlServer(connectionString);
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.Run();
