using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Thomesoft.Ui.Models;
using Tomsoft.BusinessLogic.BusinessLogic;

namespace Thomesoft.Ui.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public IActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync([FromForm] SearchProductRequest request)
        {            
            var result = await _mediator.Send(request);
            return View(result);
        }


        public async Task<IActionResult> TransactionsAsync()
        {
            var warehouses = await _mediator.Send(new GetWarehousesRequest());
            ViewData.Add("warehouses", warehouses);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TransactionsAsync([FromForm] GetTransactionsForProductRequest request)
        {
            var warehouses = await _mediator.Send(new GetWarehousesRequest());
            ViewData.Add("warehouses", warehouses);

            if (!ModelState.IsValid)
            {
                return View();
            }


            var result = await _mediator.Send(request);
            return View(result);
        }

        public async Task<IActionResult> PaymentMethodsAsync()
        {
            var warehouses = await _mediator.Send(new GetWarehousesRequest());
            ViewData.Add("warehouses", warehouses);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentMethodsAsync([FromForm] GetPaymentTypeForProductRequest request)
        {
            var warehouses = await _mediator.Send(new GetWarehousesRequest());
            ViewData.Add("warehouses", warehouses);

            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _mediator.Send(request);
            return View(result);
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}