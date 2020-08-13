using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Department>> FindAllAsync() 
        {
            // retornando uma lista de departamentos ordenados pelo nome usando linq e lambda
            return await _context.Department.OrderBy(dp => dp.Name).ToListAsync();

            // a operacao Linq é lazy e só acontece na chamado de ToList() que é síncrona
            // para torna-lá assíncrona usamos o ToListAsync (do EntityFramework)
            // além de retornar um 'async Task', informamos que ela á 'await' pro compilador. 

            // Usar operações assíncronas tem por objetivo melhorar performance em momentos de 
            // maior exigência como acesso a banco de dados, rede ou até mesmo disco.
        }

    }
}
