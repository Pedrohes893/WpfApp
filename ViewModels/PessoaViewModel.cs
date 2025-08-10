using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public class PessoaViewModel : INotifyPropertyChanged
    {
        //Quando ocorrer uma alteracao na proriedade esse evento vai ocorrer
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly PessoaService _service; // instância única

        //_nomePessoa é o valor passado através da busca
        public string _nomePessoa;
        public string NomePessoa { get => _nomePessoa; set
            {
                if (_nomePessoa != value) { 
                    _nomePessoa = value;
                    NotifyPropertyChanged("NomePessoa");
                }
            } }

        // Caso alguma pessoa esteja sendo editada
        private Pessoa _pessoaEditando;
        public Pessoa PessoaEditando
        {
            get => _pessoaEditando;
            set
            {
                _pessoaEditando = value;
                NotifyPropertyChanged(nameof(PessoaEditando));
            }
        }

        public ObservableCollection<Pessoa> Pessoas { get; set; }

        // Comando para Buscar Pessoas
        public ICommand BuscarPessoas { get; set; }

        // Comando para Incluir novas pessoas
        public ICommand IncluirPessoa { get; set; }

        // Comando para salvar
        public ICommand Salvar { get; set; }

        // Excluir
        public ICommand ExcluirPessoa { get; set; }

        // Incluir pedido
        public ICommand IncluirPedido { get; }

        public Pessoa PessoaSelecionada { get; set; } // Para bind no DataGrid/ListView

        // Construtor
        public PessoaViewModel()
        {
            _service = new PessoaService();

            BuscarPessoas = new AsyncRelayCommand(async () => await CarregarPessoasAsync(NomePessoa));
            IncluirPessoa = new RelayCommand(NovaPessoa);
            Salvar = new AsyncRelayCommand(async () => await SalvarPessoa());
            ExcluirPessoa = new RelayCommand(ExcluirPessoaSelecionada, PodeExcluirPessoa); // Novo comando
            IncluirPedido = new RelayCommand(ExecutarIncluirPedido, PodeInserirPedido);
            _ = CarregarPessoasAsync("");
        }

        private void ExecutarIncluirPedido(object parameter)
        {
            var modal = new ModalCadastroPedido();
            modal.Owner = Application.Current.Windows
                            .OfType<Window>()
                            .FirstOrDefault(w => w.IsActive);
            modal.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            modal.ShowDialog();
        }

        // Carrega as informaçoes do JSON
        public async Task CarregarPessoasAsync(string nomePessoa)
        {
            try
            {
                var lista = await _service.CarregarPessoasAsync(nomePessoa);

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

        // Inclui nova pessoa na lista
        public void NovaPessoa(object obj)
        {
            if (Pessoas != null && Pessoas.Any(p => string.IsNullOrWhiteSpace(p.NumCpf)))
            {
                MessageBox.Show("Conclua o preenchimento da pessoa atual antes de adicionar outra.",
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int novoId = Pessoas != null && Pessoas.Any() ? Pessoas.Max(p => p.IdPessoa) + 1 : 1;

            var pessoa = new Pessoa
            {
                IdPessoa = novoId,
                NomePessoa = string.Empty,
                NumCpf = string.Empty
            };

            Pessoas.Add(pessoa);
            PessoaEditando = pessoa;

            NotifyPropertyChanged(nameof(Pessoas));
            NotifyPropertyChanged(nameof(PessoaEditando));
        }

        // Salvar alteraçao nos usuarios
        public async Task SalvarPessoa()
        {
            try
            {
                await _service.SalvarPessoasAsync(Pessoas.ToList());
                MessageBox.Show("Pessoas salvas com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao salvar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool PodeExcluirPessoa(object obj)
        {
            return PessoaSelecionada != null; // Só pode excluir se houver seleção
        }

        private bool PodeInserirPedido(object obj)
        {
            return PessoaSelecionada != null; // Só pode excluir se houver seleção
        }

        // Exlcuir pessoas
        public async void ExcluirPessoaSelecionada(object obj)
        {
            try
            {
                if (PessoaSelecionada != null && Pessoas.Contains(PessoaSelecionada))
                {
                    Pessoas.Remove(PessoaSelecionada);
                    PessoaSelecionada = null;

                    await _service.SalvarPessoasAsync(Pessoas.ToList());
                    MessageBox.Show("Pessoa excluída com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                    NotifyPropertyChanged(nameof(Pessoas));
                    NotifyPropertyChanged(nameof(PessoaSelecionada));
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
