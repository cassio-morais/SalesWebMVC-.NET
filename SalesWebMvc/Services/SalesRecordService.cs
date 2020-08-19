using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }


        public async Task Insert(SalesRecord obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }


        public async Task<SalesRecord> FindByIdAsync(int id)
        {
            var obj = await _context.SalesRecord.FindAsync(id);
            return obj;

        }

        // UpdateAsync e RemoveAsync são métodos que terão que tratar possíveis erros de acesso ao banco de dados
        // concorrência ou integridade referêncial por exemplo

        public async Task UpdateAsync(SalesRecord obj)
        {
            
            if (!await _context.SalesRecord.AnyAsync(sale => sale.Id == obj.Id)) // ver SellerService UpdateAsync
            {
                throw new NotFoundException("Id not Found!"); 
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new IntegrityException(e.Message);
            }

        }


        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.SalesRecord.FindAsync(id);
                _context.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {

                throw new IntegrityException(e.Message);

            }
        }


        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj; // pega o dbset e transforma em objeto para tratarmos com o linq

            if (minDate.HasValue) // tem data minima informada 
            {
                result = result.Where(x => x.Date >= minDate.Value); // Executando restrição com objeto result criado acima
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(s => s.Seller) // pegando da composição com Seller dentro da classe SalesRecord
                .Include(d => d.Seller.Department)
                .OrderByDescending(d => d.Date)
                .ToListAsync();

        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj; // pega o dbset e transforma em objeto para tratarmos com o linq

            if (minDate.HasValue) // tem data minima informada 
            {
                result = result.Where(x => x.Date >= minDate.Value); // Executando restrição com objeto result criado acima
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(s => s.Seller) // pegando da composição com Seller dentro da classe SalesRecord
                .Include(d => d.Seller.Department)
                .OrderByDescending(d => d.Date)
                .GroupBy(g => g.Seller.Department)
                .ToListAsync();

        }


    }
}
