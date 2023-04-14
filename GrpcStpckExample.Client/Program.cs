using Grpc.Core;
using Grpc.Net.Client;
using GrpcStockExample;

internal class Program
{
    private static readonly string[] Symbols = { "MSFT", "GOOG", "AAPL", "AMZN", "META", "BABA", "Unknown" };

    public static async Task Main(string[] args)
    {
        // Create a channel to the gRPC server.
        var channel = GrpcChannel.ForAddress("http://localhost:5078");

        // Create a client to call the gRPC service.
        var client = new Stock.StockClient(channel);

        foreach (var symbol in Symbols)
            try
            {
                // Call the gRPC method.
                var reply = await client.GetStockPriceAsync(new StockRequest { Name = symbol });

                // Display the response.
                Console.WriteLine($"{symbol} price: {reply.Price}");
            }
            catch (RpcException ex)
            {
                Console.WriteLine(
                    $"Error getting price for {symbol}. Status: {ex.Status.StatusCode}. Message: {ex.Status.Detail}");
            }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}