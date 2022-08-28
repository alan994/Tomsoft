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
    public class GetPaymentTypeForProductRequestValidator : AbstractValidator<GetPaymentTypeForProductRequest>
    {
        public GetPaymentTypeForProductRequestValidator()
        {
            this.RuleFor(x => x.ProductId).NotNull().NotEmpty();
            this.RuleFor(x => x.StartDate).NotEqual(DateTime.MinValue);
        }
    }
    public class GetPaymentTypeForProductRequest : IRequest<GetPaymentTypeForProductResponse>
    {
        public string ProductId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class GetPaymentTypeForProductResponse
    {
        public List<GetPaymentTypeForProductResponse__PaymentType> PaymentTypes { get; set; } = new List<GetPaymentTypeForProductResponse__PaymentType>();
        public class GetPaymentTypeForProductResponse__PaymentType
        {            
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal Amount { get; set; }
        }
    }

    public class GetPaymentTypeForProductHandler : IRequestHandler<GetPaymentTypeForProductRequest, GetPaymentTypeForProductResponse>
    {
        private readonly ILogger<GetPaymentTypeForProductHandler> logger;
        private readonly LuceedApiClient api;

        public GetPaymentTypeForProductHandler(ILogger<GetPaymentTypeForProductHandler> logger, LuceedApiClient api)
        {
            this.logger = logger;
            this.api = api;
        }
        public async Task<GetPaymentTypeForProductResponse> Handle(GetPaymentTypeForProductRequest request, CancellationToken cancellationToken)
        {
            var apiResponse = await this.api.GetPaymentTypeForProductAsync(request.ProductId, request.StartDate, request.EndDate);
            if (apiResponse == null || apiResponse.Result == null || !apiResponse.Result.Any())
            {
                return new GetPaymentTypeForProductResponse();
            }


            return new GetPaymentTypeForProductResponse()
            {
                PaymentTypes = apiResponse.Result.FirstOrDefault().PaymentTypes.Select(x => new GetPaymentTypeForProductResponse.GetPaymentTypeForProductResponse__PaymentType()
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Name = x.Name
                }).ToList()
            };
        }
    }
}
