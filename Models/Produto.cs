using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WpfApp.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }

        public string NomeProduto { get; set; }

        public string CodigoProduto { get; set; }

        public decimal ValorProduto { get; set; }
    }
}
