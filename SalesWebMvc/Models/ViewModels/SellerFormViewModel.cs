using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Models.ViewModels
{
    public class SellerFormViewModel
    {
        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; }

        // classe criada pra montar um formulário agrupando vendedor e vários departamentos
        // vai ser injetada lá no SellerController pra fazer a tela de cadastro 
        // com um select de departments
    }
}
