using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp.Models;

namespace WpfApp.Views
{
    public partial class ItensPedido : Window
    {
        public ObservableCollection<ItemPedidoViewModel> Itens { get; set; }

        public ItensPedido(IEnumerable<ItemPedido> itens)
        {
            InitializeComponent();

            // Mapear os itens para uma ViewModel simples com subtotal
            Itens = new ObservableCollection<ItemPedidoViewModel>(
                itens.Select(i => new ItemPedidoViewModel
                {
                    Produto = i.Produto,
                    Quantidade = i.Quantidade,
                    Subtotal = i.Quantidade * i.Produto.ValorProduto
                }));

            DataContext = this;
        }
    }

    public class ItemPedidoViewModel
    {
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal { get; set; }
    }
}
