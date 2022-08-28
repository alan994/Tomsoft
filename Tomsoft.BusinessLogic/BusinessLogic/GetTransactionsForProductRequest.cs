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
    public class GetTransactionsForProductRequestValidator : AbstractValidator<GetTransactionsForProductRequest>
    {
        public GetTransactionsForProductRequestValidator()
        {
            this.RuleFor(x => x.ProductId).NotNull().NotEmpty();
            this.RuleFor(x => x.StartDate).NotEqual(DateTime.MinValue);
        }
    }
    public class GetTransactionsForProductRequest : IRequest<GetTransactionsForProductResponse>
    {
        public string ProductId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class GetTransactionsForProductResponse
    {
        public List<GetTransactionsForProductResponse__Transactions> Transactions { get; set; } = new List<GetTransactionsForProductResponse__Transactions>();
        public class GetTransactionsForProductResponse__Transactions
        {
            public string ProductId { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public decimal Amount { get; set; }
            public bool IsService { get; set; }
        }
    }

    public class GetTransactionsForProductHandler : IRequestHandler<GetTransactionsForProductRequest, GetTransactionsForProductResponse>
    {
        private readonly ILogger<GetTransactionsForProductHandler> logger;
        private readonly LuceedApiClient api;

        public GetTransactionsForProductHandler(ILogger<GetTransactionsForProductHandler> logger, LuceedApiClient api)
        {
            this.logger = logger;
            this.api = api;
        }
        public async Task<GetTransactionsForProductResponse> Handle(GetTransactionsForProductRequest request, CancellationToken cancellationToken)
        {
            var apiResponse = await this.api.GetTransactionsForProductAsync(request.ProductId, request.StartDate, request.EndDate);
            if (apiResponse == null || apiResponse.Result == null || !apiResponse.Result.Any())
            {
                return new GetTransactionsForProductResponse();
            }


            return new GetTransactionsForProductResponse()
            {
                Transactions = apiResponse.Result.FirstOrDefault().Transactions.Select(x => new GetTransactionsForProductResponse.GetTransactionsForProductResponse__Transactions()
                {
                    ProductId = x.ProductId,
                    Amount = x.Amount,
                    IsService = x.IsService,
                    Name = x.Name,
                    Quantity = x.Quantity
                }).ToList()
            };
        }
    }
}
