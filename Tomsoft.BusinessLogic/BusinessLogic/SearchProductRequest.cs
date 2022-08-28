using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomsoft.BusinessLogic.Services;

namespace Tomsoft.BusinessLogic.BusinessLogic
{    
    public class SearchProductRequest : IRequest<SearchProductResponse>
    {
        public string? Query { get; set; }
    }

    public class SearchProductResponse
    {
        public List<SearchProductResponse__Product> Products { get; set; } = new List<SearchProductResponse__Product>();
        public class SearchProductResponse__Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    }


    public class SearchProductHandler : IRequestHandler<SearchProductRequest, SearchProductResponse>
    {
        private readonly ILogger<SearchProductHandler> logger;
        private readonly LuceedApiClient api;

        public SearchProductHandler(ILogger<SearchProductHandler> logger, LuceedApiClient api)
        {
            this.logger = logger;
            this.api = api;
        }

        public async Task<SearchProductResponse> Handle(SearchProductRequest request, CancellationToken cancellationToken)
        {
            //It would be nice to have cache here...
            var apiResponse = await this.api.SearchProductAsync(request.Query);
            if(apiResponse == null || apiResponse.Result == null || !apiResponse.Result.Any())
            {
                return new SearchProductResponse();
            }

            return new SearchProductResponse()
            {
                Products = apiResponse.Result.FirstOrDefault()?.Products?.Select(x => new SearchProductResponse.SearchProductResponse__Product()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };
        }
    }
}
