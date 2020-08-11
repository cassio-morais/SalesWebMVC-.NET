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
using SalesWebMvc.Services.Exceptions;

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

        // GET - seleciona pra confirmar a deleção
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var seller = _sellerService.FindById(id.Value); // por ser opcional, vc tem que passar o Value
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }


        // POST - Deleta ao confirmar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = _sellerService.FindById(id.Value); // por ser opcional, vc tem que passar o Value
            if (seller == null)
            {
                return NotFound();
            }


            return View(seller);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_sellerService.FindById(id.Value) == null)
            {
                return NotFound();
            } 
                
            var seller = _sellerService.FindById(id.Value); // pega 1 seller pelo id
            var departments = _departmentService.FindAll(); // pega todos departamentos
            var sellerViewModel = new SellerFormViewModel { Seller = seller, Departments = departments  }; // monta um objeto composto pra jogar na tela

            return View(sellerViewModel);          

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller) // o id já vai estar na url e o obj seller virá pelo post
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }

            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
           
            catch (NotFoundException) // erro personalizado criado em services/exceptions
            {
                return NotFound();
            }
            catch (DbConcurrencyExcepction) // erro personalizado criado em services/exceptions
            {
                return BadRequest();
            }

        }

    }
}
