using System;

namespace SalesWebMvc.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; } // id interno da requisicao
        public string  Message { get; set; } // pra acrescentar uma msg customizada ao erro

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        // funcao retorna se não é nulo ou vazio
    }
}