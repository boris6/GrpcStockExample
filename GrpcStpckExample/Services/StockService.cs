using Grpc.Core;
using GrpcStockExample.Exceptions;
using GrpcStockExample.Interfaces;

namespace GrpcStockExample.Services;

public class StockService : Stock.StockBase
{
    private readonly IFinanceService _financeService;

    public StockService(IFinanceService financeService)
    {
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