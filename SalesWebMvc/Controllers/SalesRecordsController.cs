using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SalesWebMvc.Models;
using SalesWebMvc.Models.Enums;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {

        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> CreateSale()
        {
           var sellers = await _sellerService.FindAllAsync();
           var viewModel = new SalesRecordFormViewModel { Sellers = sellers };

            return View(viewModel);

       }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSale(SalesRecordFormViewModel ViewObj) // passando o viewModel por completo
        {
            if (!ModelState.IsValid)
            {
                var sellers = await _sellerService.FindAllAsync();
                var viewModel = new SalesRecordFormViewModel { Sellers = sellers };

                return View(viewModel);
            }
             
            SalesRecord obj = new SalesRecord // não consegui pegar o objeto SalesRecord no Create Sale ¯\_(ツ)_/¯
            {
                Date = ViewObj.SalesRecord.Date,
                Amount = ViewObj.SalesRecord.Amount,
                Status = ViewObj.SalesRecord.Status,
                SellerId = ViewObj.SalesRecord.SellerId
            };
            
            await _salesRecordService.Insert(obj);

            return RedirectToAction(nameof(Index));
           
        }



        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {

            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd"); // esses dicionarios já vão na chamada da view, não precisa passar como parâmetro
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateAsync(minDate.Value, maxDate.Value);
            
            return View(result);
        }


        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {

            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd"); // esses dicionarios já vão na chamada da view, não precisa passar como parâmetro
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateGroupingAsync(minDate.Value, maxDate.Value);

            return View(result);
        }


    }
}
