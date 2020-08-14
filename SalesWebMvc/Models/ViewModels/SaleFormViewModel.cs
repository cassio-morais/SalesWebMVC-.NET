using SalesWebMvc.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Models.ViewModels
{
    public class SaleFormViewModel
    {   
        public SalesRecord SalesRecord { get; set; }
        public SalesStatus SalesStatus { get; set; }
        public IEnumerable<Seller> Sellers { get; set; }

    }
}
