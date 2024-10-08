﻿using StocksApp.ServiceContracts;
using System.Text.Json;

namespace StocksApp.Services
{
    public class FinnhubService : IFinnhubService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public FinnhubService
            (IHttpClientFactory httpClientFactory,
            IConfiguration configuration) 
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };


                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);


                Stream stream = httpResponseMessage.Content.ReadAsStream();


                StreamReader streamreader = new StreamReader(stream);

                string response = streamreader.ReadToEnd();

                Dictionary<string, object>? responseDict =
                JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (responseDict == null)
                {
                    throw new InvalidOperationException("no response from finnhub");
                }

                if (responseDict.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(responseDict["error"]));
                }

                return responseDict;
            }
        }

        public async Task<Dictionary<string, object>> GetStockPriceQoute(string stockSymbol) 
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };


                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);


                Stream stream = httpResponseMessage.Content.ReadAsStream();


                StreamReader streamreader = new StreamReader(stream);

                string response = streamreader.ReadToEnd();

                Dictionary<string, object>? responseDict =
                JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (responseDict == null) {
                    throw new InvalidOperationException("no response from finnhub");
                }

                if (responseDict.ContainsKey("error")) { 
                    throw new InvalidOperationException(Convert.ToString(responseDict["error"]));
                }

                return responseDict;
            }
        }


    }
}
