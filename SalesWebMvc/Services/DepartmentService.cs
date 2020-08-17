using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context; 

        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }



        public async Task InsertAsync(Department obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }



        public async Task UpdateAsync(Department obj)
        {
            var hasAny = await _context.Department.AnyAsync(d => d.Id == obj.Id);

            if (!hasAny)
            {
                throw new NotFoundException("Id Not found");
            }

            try
            {

                _context.Department.Update(obj);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyExcepction(e.Message);

            }
        }    



        public async Task DeleteAsync(int id)
        {

            try
            {
                var obj = await _context.Department.FindAsync(id);
                _context.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            { 
                throw new IntegrityException(e.Message);

            }

        }



        public async Task<Department> FindByIdAsync(int id)
        {
            return await _context.Department.FirstOrDefaultAsync(dp => dp.Id == id);

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
