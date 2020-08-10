using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Clauses;
using SalesWebMvc.Services;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService; // declarando uma variável para dependencia do sellerService

        public SellersController(SellerService sellerService) // injetando via construtor
        {
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            var sellersList = _sellerService.FindAll();

            return View(sellersList);
        }
    }
}
