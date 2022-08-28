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
    public class GetWarehousesRequest : IRequest<GetWarehousesResponse>
    {
        public string? Query { get; set; }
    }

    public class GetWarehousesResponse
    {
        public List<GetWarehousesResponse__Warehouse> Warehouses { get; set; } = new List<GetWarehousesResponse__Warehouse>();
        public class GetWarehousesResponse__Warehouse
        {
            public string PjId { get; set; }
            public string Name { get; set; }
        }
    }


    public class GetwarehousesHandler : IRequestHandler<GetWarehousesRequest, GetWarehousesResponse>
    {
        private readonly ILogger<GetwarehousesHandler> logger;
        private readonly LuceedApiClient api;

        public GetwarehousesHandler(ILogger<GetwarehousesHandler> logger, LuceedApiClient api)
        {
            this.logger = logger;
            this.api = api;
        }

        public async Task<GetWarehousesResponse> Handle(GetWarehousesRequest request, CancellationToken cancellationToken)
        {
            var apiResponse = await this.api.GetwarehousesAsync();
            if (apiResponse == null || apiResponse.Result == null || !apiResponse.Result.Any())
            {
                return new GetWarehousesResponse();
            }

            return new GetWarehousesResponse()
            {
                Warehouses = apiResponse.Result.FirstOrDefault()?.Warehouses?.Select(x => new GetWarehousesResponse.GetWarehousesResponse__Warehouse()
                {
                    PjId = x.PjUid,
                    Name = $"{x.Name} - {x.PjName}"
                }).ToList()
            };
        }
    }
}
