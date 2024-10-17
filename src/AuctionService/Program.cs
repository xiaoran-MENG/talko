using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(o => {
    o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

try
{
    DbSeeder.SeedDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();
