using System.Text;
using System.Text.Json;
using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable;
using System.Net;

public static class RabbitMQSender
{
    public static async Task SendBidMessage()
    {
        Console.WriteLine("Initialiserer RabbitMQ StreamSystem...");
        // Opret StreamSystem-konfiguration
        var streamSystem = await StreamSystem.Create(new StreamSystemConfig
        {
            Endpoints = new List<EndPoint> { new DnsEndPoint("rabbitmq", 5552) }
        });
        // Opret en ny stream med specifikationer
        await streamSystem.CreateStream(new StreamSpec("bidding-stream")
        {
            MaxLengthBytes = 5_000_000_000
        });
        Console.WriteLine("Stream bidding-stream er påbegyndt med en længde på 5gb");

        // Opret en producer
        var producer = await Producer.Create(new ProducerConfig(streamSystem, "bidding-stream"));

        // Skab et bud-objekt
        var bid = new
        {
            ItemId = "64c3b79f8b1e5f235f9e1b2c", // Eksempel ObjectId for en genstand
            BidderId = "user123",
            BidAmount = 500.00
        };

        // Serialiser bud-objektet til JSON
        string bidMessage = JsonSerializer.Serialize(bid);

        // Send besked
        await producer.Send(new Message(Encoding.UTF8.GetBytes(bidMessage)));
        Console.WriteLine($" [x] Sent bid: {bidMessage}");

        // Afslutning
        await producer.Close();
        await streamSystem.Close();
    }
}