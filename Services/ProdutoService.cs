using System.IO;
using System.Linq;
using System.Text.Json;
using WpfApp.Models;
using WpfApp.Validacoes;

namespace WpfApp.Services
{
    internal class ProdutoService
    {
        private readonly string caminhoJson;
        public ProdutoService()
        {
            // Caminho até o diretório do projeto
            string pastaProjeto = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
            caminhoJson = Path.Combine(pastaProjeto, "Data", "produtos.json");
        }

        public async Task<List<Produto>> CarregarProdutosAsync(string nomeProduto)
        {
            try
            {
                if (!File.Exists(caminhoJson))
                    return new List<Produto>();

                string json = await File.ReadAllTextAsync(caminhoJson);
                var lista = JsonSerializer.Deserialize<List<Produto>>(json);

                if (!string.IsNullOrWhiteSpace(nomeProduto))
                {
                    // Filtra a lista, comparando o nome (ignora maiúsculas/minúsculas)
                    lista = lista
                        .Where(p => p.NomeProduto.ToUpper().Contains(nomeProduto.ToUpper()))
                        .ToList();
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Salvar lista no arquivo JSON
        public async Task SalvarProdutosAsync(List<Produto> produtos)
        {

            try
            {
                foreach (var produto in produtos)
                {
                    if (string.IsNullOrEmpty(produto.NomeProduto))
                    {
                        throw new ArgumentException("O Nome do Produto é obrigatório!");
                    }
                    if (string.IsNullOrEmpty(produto.CodigoProduto))
                    {
                        throw new ArgumentException("O Código do Produto é obrigatório!");
                    }
                    if (produto.ValorProduto == 0)
                    {
                        throw new ArgumentException("O Valor do Produto é obrigatório!");
                    }
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(produtos, options);
                await File.WriteAllTextAsync(caminhoJson, json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
