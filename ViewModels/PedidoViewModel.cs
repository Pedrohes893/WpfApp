using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Models;
using WpfApp.Services;
using WpfApp.Views;

namespace WpfApp.ViewModels
{
    internal class PedidoViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly PessoaService _pessoaService;
        private readonly PedidoService _pedidoService;
        public ObservableCollection<Pessoa> Pessoas { get; set; }
        public ObservableCollection<Pedido> Pedidos { get; set; }

        private Pessoa _pessoaSelecionada;
        public Pessoa PessoaSelecionada
        {
            get => _pessoaSelecionada;
            set
            {
                if (_pessoaSelecionada != value)
                {
                    _pessoaSelecionada = value;
                    NotifyPropertyChanged(nameof(PessoaSelecionada));
                    _ = CarregarPedidosAsync();
                }
            }
        }

        public ICommand VerItensCommand => new RelayCommand(obj =>
        {
            if (obj is Pedido pedido)
            {
                var janelaItens = new ItensPedido(pedido.Produtos);
                janelaItens.Owner = Application.Current.Windows
                                           .OfType<Window>()
                                           .FirstOrDefault(w => w.IsActive);
                janelaItens.ShowDialog();
            }
        });

        public PedidoViewModel()
        {
            _pessoaService = new PessoaService();
            _pedidoService = new PedidoService();

            Pedidos = new ObservableCollection<Pedido>();

            _ = CarregarPessoasAsync("");
        }


        // Carrega as informaçoes do JSON
        public async Task CarregarPessoasAsync(string nomePessoa)
        {
            try
            {
                var lista = await _pessoaService.CarregarPessoasAsync(nomePessoa);

                if (Pessoas == null)
                    Pessoas = new ObservableCollection<Pessoa>();
                else
                    Pessoas.Clear();

                foreach (var pessoa in lista)
                    Pessoas.Add(pessoa);

                NotifyPropertyChanged(nameof(Pessoas));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao salvar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CarregarPedidosAsync()
        {
            try
            {
                if (PessoaSelecionada == null)
                {
                    Pedidos.Clear();
                    return;
                }

                var listaPedidos = await _pedidoService.CarregarPedidosPorPessoaAsync(PessoaSelecionada.IdPessoa);

                Pedidos.Clear();
                foreach (var pedido in listaPedidos)
                    Pedidos.Add(pedido);

                NotifyPropertyChanged(nameof(Pedidos));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar pedidos: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NotifyPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
