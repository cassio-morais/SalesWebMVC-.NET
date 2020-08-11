using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services.Exceptions
{
    //Criando uma exceção personalizada herdando de ApplicationException
    public class DbConcurrencyExcepction : ApplicationException
    {
        public DbConcurrencyExcepction(string message) : base(message) // construtor herdando message de ApplicationException
        {

        }
    }
}
