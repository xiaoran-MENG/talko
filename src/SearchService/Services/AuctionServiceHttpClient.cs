using System;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionServiceHttpClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public AuctionServiceHttpClient(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _config = configuration;
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(x => x.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();
        
        var url = _config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated;

        return await _http.GetFromJsonAsync<List<Item>>(url);
    }
}
