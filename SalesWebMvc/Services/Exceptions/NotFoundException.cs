using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        //Criando uma exceção personalizada herdando de ApplicationException
        public NotFoundException(string message) : base(message) // construtor herdando message de ApplicationException
        {

        }

    }
}
