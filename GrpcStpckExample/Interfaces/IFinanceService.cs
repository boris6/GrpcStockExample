namespace GrpcStockExample.Interfaces;

public interface IFinanceService
{
    Task<double> GetStockPrice(string stockName);
}