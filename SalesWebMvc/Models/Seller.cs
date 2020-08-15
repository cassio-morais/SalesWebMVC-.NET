﻿using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }

        //Obrigatório            |Nome do atributo
        [Required(ErrorMessage = "{0} required")]                                   // min      max   
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} Size should be between {2} and {1}")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")] // validador de email com msg de erro
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [Display(Name = "Birth Date")] // Data Anotation pra um nome amigável ao renderizar
        [DataType(DataType.Date)] // formato data aparecendo apenas o ano
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] // formatando padrão brasileiro
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")] // 0 : atribudo, F2 : duas casas decimais
        public double BaseSalary { get; set; }

        public Department Department { get; set; }
        public int DepartmentId { get; set; } // integridade referencial
        // por padrão ao criar um tipo int de DepartmentId o .net já correlaciona isso no
        // banco de dados

        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {
        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }


        public void AddSales(SalesRecord salesRecord)
        {
            Sales.Add(salesRecord);
        }


        public void RemoveSales(SalesRecord salesRecord)
        {
            Sales.Remove(salesRecord);
        }


        public double TotalSales(DateTime initial, DateTime final)
        {
            var total = Sales
                .Where(sale => sale.Date >= initial && sale.Date <= final)
                .Sum(sale => sale.Amount);
                
            return total; 
        }


    }
}
