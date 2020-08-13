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
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;


namespace SalesWebMvc.controllers
{
    public class SellersController : Controller
    {

        private readonly SellerService _sellerservice; // declarando uma variável para dependencia do sellerservice
        private readonly DepartmentService _departmentservice;


        public SellersController(SellerService sellerservice, DepartmentService departmentservice) // injetando via construtor
        {
            _sellerservice = sellerservice;
            _departmentservice = departmentservice;
        }

        public async Task<IActionResult> Index()
        {
            var sellerslist = await _sellerservice.FindAllAsync();

            return View(sellerslist);
        }


        // get
        public async Task<IActionResult> Create()
        {

            var departments = await _departmentservice.FindAllAsync();
            var sellerviewmodel = new SellerFormViewModel { Departments = departments };
            // cria-se um objeto agrupado que já passa pra view a lista de departments para um select no html
            // e nele vai junto um atributo seller que será preenchido pelo formulário

            return View(sellerviewmodel);
        }


        // post
        [HttpPost] // metodo post pegando o formulário enviado
        [ValidateAntiForgeryToken] // token anti csrf
        public async Task<IActionResult> Create(Seller seller) // recebe o objeto no pos
        {
            if (!ModelState.IsValid) // valida a nível de backend se o objeto está correto de acordo com as restrições da classe
            {
                var departments = await _departmentservice.FindAllAsync();
                var sellerviewmodel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(sellerviewmodel); // retorna o objeto completo
            }

            await _sellerservice.Insert(seller); // chama o insert do sellerservice pra inserir no banco

            return RedirectToAction(nameof(Index)); // retorna para a index
        }





        // get - seleciona pra confirmar a deleção
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "id not provided" });
            }

            var seller = await _sellerservice.FindByIdAsync(id.Value); // por ser opcional, vc tem que passar o value
            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "id not found" });
            }

            return View(seller);
        }


        // post - deleta ao confirmar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerservice.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not provided" });
            }

            var seller = await _sellerservice.FindByIdAsync(id.Value); // por ser opcional, vc tem que passar o value
            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not found" }); ;
            }


            return View(seller);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not provided" });
            }

            if (await _sellerservice.FindByIdAsync(id.Value) == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not found" });
            }

            var seller = await _sellerservice.FindByIdAsync(id.Value); // pega 1 seller pelo id
            var departments = await _departmentservice.FindAllAsync(); // pega todos departamentos
            var sellerviewmodel = new SellerFormViewModel { Seller = seller, Departments = departments }; // monta um objeto composto pra jogar na tela

            return View(sellerviewmodel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller) // o id já vai estar na url e o obj seller virá pelo post
        {

            if (!ModelState.IsValid) // valida a nível de backend se o objeto está correto de acordo com as restrições da classe
            {
                var departments = await _departmentservice.FindAllAsync();
                var sellerviewmodel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(sellerviewmodel); // retorna o objeto completo
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { Message = "id mismatch" });
            }

            try
            {
                await _sellerservice.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }

            catch (ApplicationException e) // agora usando a super-classe dos erros personalizados (na chamada farão upcasting)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }


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
