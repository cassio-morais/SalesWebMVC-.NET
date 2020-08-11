using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;

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
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id); // retornando o primeiro ou padrão
                                // funcao do EF para fazer um join com a tabela departamento
        }

        public void Remove(int? id) 
        {
            var obj = _context.Seller.Find(id); // encontra o objeto Seller
            _context.Seller.Remove(obj); // remove 
            _context.SaveChanges(); // salva a mudança

        }

        public void Update(Seller obj)
        {
            // retorna TRUE se determinado objeto NÃO existe na base de dados
            if (!_context.Seller.Any(x => x.Id == obj.Id)) {

                throw new NotFoundException("Id not Found!"); // usando a classe de serviço de exceção criada

            }
            try { 

                _context.Seller.Update(obj);
                _context.SaveChanges();

            } catch (DbUpdateConcurrencyException e) // erro de acesso ao banco em nível de concorrência
            {
                throw new DbConcurrencyExcepction(e.Message); // jogando a exceção a nível de aplicação para o controller.

                // o motivo de pegar uma exceção em nível de Banco de dados e lançar uma exceção personalizada
                // a nível de aplicação, que é a que criamos em Seller Service, é para delimitarmos as camadas
                // de acesso a informação, desse modo o controllador ao receber esse erro de volta, não estará
                // recebendo do banco, e sim da aplicação.
            }

        }



    }
}
