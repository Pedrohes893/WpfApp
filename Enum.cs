using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class Enum
    {
        public enum FormaPagamento
        {
            Dinheiro,
            Cartao,
            Boleto
        }

        public enum StatusPedido
        {
            Pendente,
            Pago,
            Enviado,
            Recebido
        }
    }
}
