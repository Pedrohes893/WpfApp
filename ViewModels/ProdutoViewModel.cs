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

namespace WpfApp.ViewModels
{
    public class ProdutoViewModel : INotifyPropertyChanged
    {
        //Quando ocorrer uma alteracao na proriedade esse evento vai ocorrer
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly ProdutoService _service; // instância única

        //_nomeProduto é o valor passado através da busca
        public string _nomeProduto;
        public string NomeProduto
        {
            get => _nomeProduto; set
            {
                if (_nomeProduto != value)
                {
                    _nomeProduto = value;
                    NotifyPropertyChanged("NomeProduto");
                }
            }
        }

        // Caso algum produto esteja sendo editado
        private Produto _produtoEditando;
        public Produto ProdutoEditando
        {
            get => _produtoEditando;
            set
            {
                _produtoEditando = value;
                NotifyPropertyChanged(nameof(ProdutoEditando));
            }
        }

        public ObservableCollection<Produto> _produtos;
        public ObservableCollection<Produto> Produtos
        {
            get => _produtos;
            set
            {
                _produtos = value;
                NotifyPropertyChanged(nameof(Produtos));
            }
        }

        // Comando para Buscar Produtos
        public ICommand BuscarProdutos { get; set; }

        // Comando para Incluir novos produtos
        public ICommand IncluirProduto { get; set; }

        // Comando para salvar
        public ICommand Salvar { get; set; }

        // Excluir
        public ICommand ExcluirProduto { get; set; }

        public Produto ProdutoSelecionado { get; set; } // Para bind no DataGrid/ListView

        // Construtor
        public ProdutoViewModel()
        {
            _service = new ProdutoService();

            BuscarProdutos = new AsyncRelayCommand(async () => await CarregarProdutosAsync(NomeProduto));
            IncluirProduto = new RelayCommand(NovoProduto);
            Salvar = new AsyncRelayCommand(async () => await SalvarProdutos());
            ExcluirProduto = new RelayCommand(ExcluirProdutoSelecionado, PodeExcluirProduto); // Novo comando
            _ = CarregarProdutosAsync("");
        }

        // Carrega as informaçoes do JSON
        public async Task CarregarProdutosAsync(string nomeProduto)
        {
            try
            {
                var produtos = await _service.CarregarProdutosAsync(NomeProduto);

                if (Produtos == null)
                    Produtos = new ObservableCollection<Produto>();
                else
                    Produtos.Clear();

                foreach (var produto in produtos)
                    Produtos.Add(produto);

                NotifyPropertyChanged(nameof(Produtos));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao salvar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Inclui novo produto na lista
        public void NovoProduto(object obj)
        {
            if (Produtos != null && Produtos.Any(p => string.IsNullOrWhiteSpace(p.NomeProduto)))
            {
                MessageBox.Show("Conclua o preenchimento do produto atual antes de adicionar outro.",
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int novoId = Produtos != null && Produtos.Any() ? Produtos.Max(p => p.IdProduto) + 1 : 1;

            var produto = new Produto
            {
                IdProduto = novoId,
                NomeProduto = string.Empty,
                CodigoProduto = string.Empty,
                ValorProduto = 0
            };

            Produtos.Add(produto);
            ProdutoEditando = produto;

            NotifyPropertyChanged(nameof(Produtos));
            NotifyPropertyChanged(nameof(ProdutoEditando));
        }

        // Salvar alteraçao nos produtos
        public async Task SalvarProdutos()
        {
            try
            {
                await _service.SalvarProdutosAsync(Produtos.ToList());
                MessageBox.Show("Produto salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao salvar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool PodeExcluirProduto(object obj)
        {
            return ProdutoSelecionado != null; // Só pode excluir se houver seleção
        }

        // Exlcuir produtos
        public async void ExcluirProdutoSelecionado(object obj)
        {
            try
            {
                if (ProdutoSelecionado != null && Produtos.Contains(ProdutoSelecionado))
                {
                    Produtos.Remove(ProdutoSelecionado);
                    ProdutoSelecionado = null;

                    await _service.SalvarProdutosAsync(Produtos.ToList());
                    MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                    NotifyPropertyChanged(nameof(Produtos));
                    NotifyPropertyChanged(nameof(ProdutoSelecionado));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao salvar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NotifyPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
