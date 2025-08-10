using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static WpfApp.Enum;

namespace WpfApp.Models
{
    public class Pedido
    {
        public int IdPedido { get; private set; }
        public int IdPessoa{ get; set; }
        public List<ItemPedido> Produtos { get; set; } = new List<ItemPedido>();
        public decimal ValorTotal => Produtos?.Sum(p => p.Produto.ValorProduto * p.Quantidade) ?? 0;
        public DateTime DataVenda { get; set; } = DateTime.Now;
        public FormaPagamento FormaPagamento { get; set; }
        public StatusPedido Status { get; set; } = StatusPedido.Pendente;
    }

    public class ItemPedido
    {
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
    }

}
