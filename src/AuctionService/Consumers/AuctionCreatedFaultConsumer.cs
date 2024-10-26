using System;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        System.Console.WriteLine("Consuming auction created fault | auction service");
        var e = context.Message.Exceptions.First();
        if (e.ExceptionType == "System.ArgumentException")
        {
            context.Message.Message.Model = "FooUpdated";
            await context.Publish(context.Message.Message);
        }
    }
}
