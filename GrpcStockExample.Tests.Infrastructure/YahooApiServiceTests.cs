using System.Net;
using GrpcStockExample.Exceptions;
using GrpcStockExample.Infrastructure;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace GrpcStockExample.Tests.Infrastructure;

public class YahooApiServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly YahooApiService _yahooApiService;

    public YahooApiServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _yahooApiService = new YahooApiService(httpClient);
    }

    [Fact]
    public async Task GetStockPrice_WhenStockNameIsKnown_ReturnsStockPrice()
    {
        // Arrange
        var stockName = "MSFT";
        var stockPrice = 100.0;
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(new
            {
                quoteSummary = new
                {
                    result = new[]
                    {
                        new
                        {
                            financialData = new
                            {
                                currentPrice = new
                                {
                                    raw = stockPrice
                                }
                            }
                        }
                    }
                }
            }))
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        // Act
        var actualResponse = await _yahooApiService.GetStockPrice(stockName);

        // Assert
        Assert.Equal(stockPrice, actualResponse);
    }

    [Fact]
    public async Task GetStockPrice_WhenStockNameIsUnknown_ThrowsSymbolNotFoundException()
    {
        // Arrange
        var stockName = "MSFT";
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        // Act
        var exception = await Assert.ThrowsAsync<SymbolNotFoundException>(() =>
            _yahooApiService.GetStockPrice(stockName));

        // Assert

        Assert.Equal("Stock price not found", exception.Message);
    }

    [Fact]
    public async Task GetStockPrice_WhenResponseStatusCodeIsNotSuccess_ThrowsException()
    {
        // Arrange
        var stockName = "MSFT";
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _yahooApiService.GetStockPrice(stockName));

        // Assert

        Assert.Equal("Response status code does not indicate success: 500 (Internal Server Error).",
            exception.Message);
    }
}