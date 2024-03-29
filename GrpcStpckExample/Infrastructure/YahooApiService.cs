﻿using System.Net;
using GrpcStockExample.Exceptions;
using GrpcStockExample.Interfaces;
using Newtonsoft.Json;

namespace GrpcStockExample.Infrastructure;

public class YahooApiService : IFinanceService
{
    private readonly HttpClient _httpClient;

    public readonly string YahooFinanceApiBaseUrl =
        "https://query1.finance.yahoo.com/v11/finance/quoteSummary/{0}?modules=financialData";

    public YahooApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<double> GetStockPrice(string stockName)
    {
        var url = string.Format(YahooFinanceApiBaseUrl, stockName);
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(content);

            var result = data.quoteSummary.result[0].financialData.currentPrice.raw;

            return (double)result;
        }

        if (response.StatusCode == HttpStatusCode.NotFound) throw new SymbolNotFoundException("Stock price not found");

        throw new Exception(string.Format("Response status code does not indicate success: {0} ({1}).",
            (int)response.StatusCode, response.ReasonPhrase));
    }
}