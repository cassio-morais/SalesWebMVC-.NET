using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {

        private readonly DepartmentService _departmentService;


        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }



        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.FindAllAsync();
            return View(departments);
        }



        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }



        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided" });
            }

            var department = await _departmentService.FindByIdAsync(id.Value);

            if (department == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found" });
            }

            return View(department);
        }



        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {

            if (!ModelState.IsValid) // não pode vir vazio [Required]
            {
                return View(department);
            }

            await _departmentService.InsertAsync(department);
            return RedirectToAction(nameof(Index));

        }


        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null) // testa nulo
            {
                return RedirectToAction(nameof(Error), new { message = "id Not Provided" });
            }

            if (await _departmentService.FindByIdAsync(id.Value) == null) // testa se foi no banco e não achou
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found" });
            }

            var department = await _departmentService.FindByIdAsync(id.Value);
            return View(department);

        }



        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id)
            {
                return RedirectToAction(nameof(Index), new { message = "Id Not Found" });
            }

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            try
            {
                await _departmentService.UpdateAsync(department);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

            return RedirectToAction(nameof(Index));

        }


        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id Not Provided" });
            }

            var department = await _departmentService.FindByIdAsync(id.Value);

            if (department == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found" });
            }

            return View(department);

        }

        // POST: Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departmentService.DeleteAsync(id);

            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

            return RedirectToAction(nameof(Index));

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

