using Newtonsoft.Json;

namespace Tomsoft.BusinessLogic.Services
{
    public class LuceedApiClient
    {
        private readonly HttpClient http;

        public LuceedApiClient(IHttpClientFactory httpClientFactory)
        {
            this.http = httpClientFactory.CreateClient("luceed");
        }

        public async Task<LuceedApiClient__SearchProductResponse> SearchProductAsync(string query)
        {
            var httpResponse = await this.http.GetAsync($"/datasnap/rest/artikli/naziv/{query}");
            httpResponse.EnsureSuccessStatusCode();                        
            return JsonConvert.DeserializeObject<LuceedApiClient__SearchProductResponse>(await httpResponse.Content.ReadAsStringAsync());
        }

        public async Task<LuceedApiClient__GetPaymentTypeForProductResponse> GetPaymentTypeForProductAsync(string productId, DateTime startDate, DateTime? endDate)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException(nameof(productId), "Product Id is required");
            }

            if(startDate == DateTime.MinValue)
            {
                throw new ArgumentNullException(nameof(startDate));
            }


            var httpResponse = await this.http.GetAsync($"/datasnap/rest/mpobracun/placanja/{productId}/{startDate.ToString("d.M.yyyy")}");
            httpResponse.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<LuceedApiClient__GetPaymentTypeForProductResponse>(await httpResponse.Content.ReadAsStringAsync());
        }

        public async Task<LuceedApiClient__GetTransactionsForProductResponse> GetTransactionsForProductAsync(string productId, DateTime startDate, DateTime? endDate)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException(nameof(productId), "Product Id is required");
            }

            if (startDate == DateTime.MinValue)
            {
                throw new ArgumentNullException(nameof(startDate));
            }


            var httpResponse = await this.http.GetAsync($"/datasnap/rest/mpobracun/artikli/{productId}/{startDate.ToString("d.M.yyyy")}");
            httpResponse.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<LuceedApiClient__GetTransactionsForProductResponse>(await httpResponse.Content.ReadAsStringAsync());
        }

        public async Task<LuceedApiClient__GetwarehousesResponse> GetwarehousesAsync()
        {            
            var httpResponse = await this.http.GetAsync($"/datasnap/rest/skladista/lista");
            httpResponse.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<LuceedApiClient__GetwarehousesResponse>(await httpResponse.Content.ReadAsStringAsync());
        }
    }

    public class LuceedApiClient__SearchProductResponse
    {
        [JsonProperty("result")]
        public List<SearchProductResponse__Result> Result { get; set; }

        public class SearchProductResponse__Result
        {
            [JsonProperty("artikli")]
            public List<SearchProductResponse__Result__Product> Products { get; set; }
        }
        public class SearchProductResponse__Result__Product
        {
            [JsonProperty("artikl_uid")]
            public string Id { get; set; }

            [JsonProperty("naziv")]
            public string Name { get; set; }
        }
    }


    public class LuceedApiClient__GetPaymentTypeForProductResponse
    {
        [JsonProperty("result")]
        public List<GetPaymentTypeForProductResponse__Result> Result { get; set; }

        public class GetPaymentTypeForProductResponse__Result
        {
            [JsonProperty("obracun_placanja")]
            public List<GetPaymentTypeForProductResponse__Result__PaymentType> PaymentTypes { get; set; }
        }
        public class GetPaymentTypeForProductResponse__Result__PaymentType
        {
            [JsonProperty("vrste_placanja_uid")]
            public string Id { get; set; }

            [JsonProperty("naziv")]
            public string Name { get; set; }

            [JsonProperty("iznos")]
            public decimal Amount { get; set; }
        }
    }

    public class LuceedApiClient__GetTransactionsForProductResponse
    {
        [JsonProperty("result")]
        public List<GetTransactionsForProductResponse__Result> Result { get; set; }

        public class GetTransactionsForProductResponse__Result
        {
            [JsonProperty("obracun_artikli")]
            public List<GetTransactionsForProductResponse__Result__Transactions> Transactions { get; set; }
        }
        public class GetTransactionsForProductResponse__Result__Transactions
        {
            [JsonProperty("artikl_uid")]
            public string ProductId { get; set; }

            [JsonProperty("naziv_artikla")]
            public string Name { get; set; }

            [JsonProperty("kolicina")]
            public int Quantity { get; set; }

            [JsonProperty("iznos")]
            public decimal Amount { get; set; }

            [JsonProperty("usluga")]
            public char IsServiceFlag { get; set; }

            public bool IsService { 
                get
                {
                    return IsServiceFlag == 'D' || IsServiceFlag == 'd';
                }
            }
        }
    }

    public class LuceedApiClient__GetwarehousesResponse
    {
        [JsonProperty("result")]
        public List<GetwarehousesResponse__Result> Result { get; set; }

        public class GetwarehousesResponse__Result
        {
            [JsonProperty("skladista")]
            public List<GetwarehousesResponse__Result__Warehouse> Warehouses { get; set; }
        }
        public class GetwarehousesResponse__Result__Warehouse
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("sid")]
            public string SId { get; set; }

            [JsonProperty("skladiste_uid")]
            public string WarehouseUid { get; set; }

            [JsonProperty("skladiste")]
            public string Warehouse { get; set; }

            [JsonProperty("naziv")]
            public string Name { get; set; }

            [JsonProperty("pj_uid")]
            public string PjUid { get; set; }
            
            [JsonProperty("pj")]
            public string Pj { get; set; }

            [JsonProperty("pj_naziv")]
            public string PjName { get; set; }

        }
    }

}
