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

        public async Task<List<Seller>> FindAllAsync()
        {
            // acessa a tabela seller e converte para uma lista
            return await _context.Seller.ToListAsync();

            // explicado em "DerpartmentService"
        }

        public async Task Insert(Seller obj) // retira o void, como não retorna nada, vc não encapsula o retorn em task
        {
            _context.Add(obj); // add o obj Seller ao contexto de banco de dados
            await _context.SaveChangesAsync(); // salva as mudanças usando o método async
            // OBS.: no _context.Add(obj) ele apenas pega em memória, por isso só usamos na parte de gravar o await
        }

        public async Task<Seller> FindByIdAsync(int id)
        {                                                              // as operações async são aquelas q realmente acessam o banco
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id); // retornando o primeiro ou padrão
                                // funcao do EF para fazer um join com a tabela departamento
                                                                            

        }

        public async Task RemoveAsync(int? id) 
        {
            var obj = await _context.Seller.FindAsync(id); // encontra o objeto Seller(vai no banco)
            _context.Seller.Remove(obj); // remove 
            await _context.SaveChangesAsync(); // salva a mudança(vai no banco)

        }

        public async Task UpdateAsync(Seller obj)
        {
            bool HasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id); // retorna TRUE se determinado objeto NÃO existe na base de dados

            // testa o retorno
            if (!HasAny)
            { 

                throw new NotFoundException("Id not Found!"); // usando a classe de serviço de exceção criada

            }
            try { 

                _context.Seller.Update(obj);
                await _context.SaveChangesAsync();

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
