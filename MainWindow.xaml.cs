using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.Views;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            main.Content = new CadastroPessoa();
            main.Content = new CadastroProduto();
        }

        private void CadastroPessoa_Click(object sender, RoutedEventArgs e)
        {
            // Navega para a Page CadastroPessoa
            main.Navigate(new Views.CadastroPessoa());
        }

        private void CadastroProdutos_Click(object sender, RoutedEventArgs e)
        {
            // Navega para a Page CadastroProduto
            main.Navigate(new Views.CadastroProduto());
        }

        private void CadastroPedidos_Click(object sender, RoutedEventArgs e)
        {
            // Navega para a Page CadastroProduto
            main.Navigate(new Views.CadastroPedido());
        }
    }
}