using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {

        private readonly SalesRecordService _salesRecord;

        public SalesRecordsController(SalesRecordService salesRecord)
        {
            _salesRecord = salesRecord;
        }

        public IActionResult Index()
        {
            return View();
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

            var result = await _salesRecord.FindByDate(minDate.Value, maxDate.Value);
            
            return View(result);
        }


        public IActionResult GroupingSearch()
        {
            return View();
        }


    }
}
