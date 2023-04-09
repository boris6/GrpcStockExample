namespace GrpcStockExample.Exceptions;

public class SymbolNotFoundException : Exception
{
    public SymbolNotFoundException(string message) : base(message)
    {
    }
}