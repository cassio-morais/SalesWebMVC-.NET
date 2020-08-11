using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context; // contexto do banco de dados

        public DepartmentService(SalesWebMvcContext context) // injeção
        {
            _context = context;
        }

        public List<Department> FindAll()
        {
            // retornando uma lista de departamentos ordenados pelo nome usando linq e lambda
            return _context.Department.OrderBy(dp => dp.Name).ToList();
        }

    }
}
