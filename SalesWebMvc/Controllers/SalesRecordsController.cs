using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.Enums;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {

        private readonly SalesRecordService _salesRecord;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesRecordService salesRecord, SellerService sellerService)
        {
            _salesRecord = salesRecord;
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> CreateSale()
        {
            var sellers = await _sellerService.FindAllAsync();
           var viewModel = new SaleFormViewModel { Sellers = sellers };

            return View(viewModel);

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

            var result = await _salesRecord.FindByDateAsync(minDate.Value, maxDate.Value);
            
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

            var result = await _salesRecord.FindByDateGroupingAsync(minDate.Value, maxDate.Value);

            return View(result);
        }


    }
}
