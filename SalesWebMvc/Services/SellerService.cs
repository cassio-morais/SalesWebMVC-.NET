using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context; // como será manipulado o banco, precisamos colocar essa dependência

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            // acessa a tabela seller e converte para uma lista
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj)
        {
            _context.Add(obj); // add o obj Seller ao contexto de banco de dados
            _context.SaveChanges(); // salva as mudanças
        }

        public Seller FindById(int id)
        {
            return _context.Seller.FirstOrDefault(seller => seller.Id == id); // retornando o primeiro ou padrão
      
        }

        public void Remove(int? id) 
        {
            var obj = _context.Seller.Find(id); // encontra o objeto Seller
            _context.Seller.Remove(obj); // remove 
            _context.SaveChanges(); // salva a mudança

        }
    }
}
