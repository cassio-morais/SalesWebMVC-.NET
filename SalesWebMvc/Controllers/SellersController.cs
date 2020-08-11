using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Clauses;
using SalesWebMvc.Services;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService; // declarando uma variável para dependencia do sellerService
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService) // injetando via construtor
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var sellersList = _sellerService.FindAll();

            return View(sellersList);
        }

        
        // GET
        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var sellerViewModel = new SellerFormViewModel { Departments = departments  };
            // cria-se um objeto agrupado que já passa pra view a lista de departments para um Select no HTML
            // e nele vai junto um atributo Seller que será preenchido pelo formulário
            
            
            return View(sellerViewModel);
        }


        // POST
        [HttpPost] // metodo post pegando o formulário enviado
        [ValidateAntiForgeryToken] // token anti CSRF
        public IActionResult Create(Seller seller) // recebe o objeto no post
        {
            _sellerService.Insert(seller); // chama o Insert do SellerService pra inserir no banco

            return RedirectToAction(nameof(Index)); // retorna para a index
        }

    }
}
