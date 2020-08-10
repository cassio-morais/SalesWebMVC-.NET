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

        
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost] // metodo post pegando o formulário enviado
        [ValidateAntiForgeryToken] // token anti CSRF
        public IActionResult Create(Seller seller) // recebe o objeto no post
        {
            _sellerService.Insert(seller); // chama o Insert do SellerService pra inserir no banco

            return RedirectToAction(nameof(Index)); // retorna para a index
        }

    }
}
