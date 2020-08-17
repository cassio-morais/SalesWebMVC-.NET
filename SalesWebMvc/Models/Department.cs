using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        // Icollection: colecao generica que engloba listas, hashset... 
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();


        public Department()
        {
        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }



        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }


        public double TotalSales(DateTime initial, DateTime final)
        {
            var total = Sellers
                .Sum(seller => seller.TotalSales(initial, final));
            // reaproveitando o total de vendas de cada vendedor

            return total;
        } 
    
    }
}
