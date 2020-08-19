using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly DepartmentService _departmentService;

        public SalesRecordsController(SalesRecordService salesRecordService,
                                        SellerService sellerService,
                                        DepartmentService departmentService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
            _departmentService = departmentService;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> CreateSale()
        {
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(SalesStatus)));
            ViewBag.Sellers = new SelectList(await _sellerService.FindAllAsync(), "Id", "Name");

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSale(SalesRecord obj) // passando o viewModel por completo
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Status = new SelectList(Enum.GetValues(typeof(SalesStatus)));
                ViewBag.Sellers = new SelectList(await _sellerService.FindAllAsync(), "Id", "Name", obj.SellerId);

                return View();

            }

            await _salesRecordService.Insert(obj);
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> EditSale(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "id mismatch" });
            }

            var obj = await _salesRecordService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Sale Not Found" });
            }

            ViewBag.Status = new SelectList(Enum.GetValues(typeof(SalesStatus)));
            ViewBag.Sellers = new SelectList(await _sellerService.FindAllAsync(), "Id", "Name");

            return View(obj);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSale(int id, SalesRecord obj)
        {
            if (id != obj.Id)
            {
                return RedirectToAction(nameof(Error), new { Message = "id mismatch" });
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Status = new SelectList(Enum.GetValues(typeof(SalesStatus)), obj.Status);
                ViewBag.Sellers = new SelectList(await _sellerService.FindAllAsync(), "Id", "Name", obj.Seller);
                return View(obj);
            }

            try
            {
                await _salesRecordService.UpdateAsync(obj);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return View(nameof(Error), new { Message = e.Message });
            }


        }


        public async Task<IActionResult> DetailsSale(int? id)
        {

            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "id mismatch" });
            }

            var obj = await _salesRecordService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Sale Not Found" });
            }

            ViewBag.Seller = await _sellerService.FindByIdAsync(obj.SellerId);
            ViewBag.Department = await _departmentService.FindByIdAsync(obj.Seller.DepartmentId);

            return View(obj);
        }



        public async Task<IActionResult> DeleteSale(int? id)
        {

            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id Mismatch" });
            }

            var obj = await _salesRecordService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Sale Not Found" });
            }

            ViewBag.Seller = await _sellerService.FindByIdAsync(obj.SellerId);
            ViewBag.Department = await _departmentService.FindByIdAsync(obj.Seller.DepartmentId);

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSale(int id)
        {
            try
            {
                await _salesRecordService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return View(nameof(Error), new { e.Message });
            }

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


        public IActionResult Error(string message) // erro não se torna assíncrono pq, nesse caso, ele não faz nenhuma operação no banco
        {
            var errorviewmodel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message
            };

            return View(errorviewmodel);

        }


    }
}
