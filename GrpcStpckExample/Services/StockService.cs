using Grpc.Core;
using GrpcStockExample.Exceptions;
using GrpcStockExample.Interfaces;

namespace GrpcStockExample.Services;

public class StockService : Stock.StockBase
{
    private readonly IFinanceService _financeService;
    private readonly ILogger<StockService> _logger;

    public StockService(ILogger<StockService> logger, IFinanceService financeService)
    {
        _logger = logger;
        _financeService = financeService;
    }

    public override async Task<StockReply> GetStockPrice(StockRequest request, ServerCallContext context)
    {
        try
        {
            var stockReply = new StockReply
            {
                Price = await _financeService.GetStockPrice(request.Name)
            };
            return stockReply;
        }
        catch (SymbolNotFoundException e)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Stock price not found"));
        }
    }
}