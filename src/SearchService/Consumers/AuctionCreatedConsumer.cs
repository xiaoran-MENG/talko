using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;

    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        System.Console.WriteLine("Consuming auction created | search service");
        var item = _mapper.Map<Item>(context.Message);
        if (item.Model == "Foo") throw new ArgumentException("Can't sell cars named Foo");
        await item.SaveAsync();
        // When the consumer throws an error
        // Rabbitmq send the message to search-auction-created_error queue for retry
    }
}
