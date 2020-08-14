using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
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

        public async Task<List<SalesRecord>> FindByDate(DateTime? minDate, DateTime? maxDate)
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



    }
}
