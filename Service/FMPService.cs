using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using stockapi.DTO.Stocks;
using stockapi.Interface;
using stockapi.Mapper;
using stockapi.Models;

namespace stockapi.Service
{
    public class FMPService : IFinancialService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        public FMPService(HttpClient http, IConfiguration config)
        {
            httpClient = http;
            configuration = config;
        }
        public async Task<Stock?> FindStockByName(string Symbol)
        {
            try
            {
                string key = configuration["FMPKey"];
                var result = await httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{Symbol}?apikey={key}");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock = tasks[0];
                    if (stock != null)
                    {
                        return stock.CreateStockFromFMPservice();
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // TODO
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}