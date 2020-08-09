using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;

namespace SalesWebMvc.Controllers
{
    public class HomeController : Controller
    {

        // 
        public IActionResult Index() 
        {
            return View();
        }

        
        // o IActionResult é uma interface, um super tipo para todo resultado de alguma action
        // 
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            // dicionário 
            // ex.:
            ViewData["email"] = "cassiomoraisg@hotmail.com";

            return View(); 
            // method Builder que retorna um objeto IActionResult, no caso View()
            // ao instanciar a View ele vai procurar na pasta Views/Home/ uma página about.cshtml (baseado na action) 
            // url: endereço/home/about
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
