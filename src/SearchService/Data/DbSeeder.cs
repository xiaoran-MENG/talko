using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

public class DbSeeder
{
    public static async Task SeedDb(WebApplication app)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(
            app.Configuration.GetConnectionString("MongoDbConnection")
        ));

        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();

        using var scope = app.Services.CreateAsyncScope();
        var http = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();
        var items = await http.GetItemsAsync();
        Console.WriteLine(items.Count + " returned from auction service");
        if (items.Count > 0) await DB.SaveAsync(items);
    }
}
