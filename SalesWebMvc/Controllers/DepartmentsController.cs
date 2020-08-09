using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            // vamos instanciar uma lista de Departments de Models/Departments.cs

            List<Department> DepartmentList = new List<Department>();

            DepartmentList.Add(new Department { Id = 1, Name = "Eletronics"}); // instanciação sem construtor
            DepartmentList.Add(new Department { Id = 2, Name = "Fashion" });
            
            return View(DepartmentList); // enviando do controlador a lista de departamentos para a View 

            // CRIANDO A VIEW: 
            // criar uma subpasta como o nome do controller "Departments" na pasta Views. Ex.:/Departments
            // depois de criar a pasta: botão direito em views/Departments > add view > Razor View

            // name: index
            // template: list
            // model class: Department (associando a view a um model)
            // add 

        }
    }
}
