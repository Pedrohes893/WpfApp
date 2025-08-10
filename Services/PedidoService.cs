using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfApp.Models;

namespace WpfApp.Services
{
    internal class PedidoService
    {
        private readonly string caminhoJson;
        public PedidoService()
        {
            // Caminho até o diretório do projeto
            string pastaProjeto = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
            caminhoJson = Path.Combine(pastaProjeto, "Data", "pedidos.json");
        }

        public async Task<List<Pedido>> CarregarPedidosPorPessoaAsync(int idPessoa)
        {
            try
            {
                if (!File.Exists(caminhoJson))
                    return new List<Pedido>();

                string json = await File.ReadAllTextAsync(caminhoJson);
                var listaPedidos = JsonSerializer.Deserialize<List<Pedido>>(json);

                if (listaPedidos == null)
                    return new List<Pedido>();

                // Filtra pedidos pelo IdPessoa
                var pedidosDoUsuario = listaPedidos
                    .Where(p => p.IdPessoa != null && p.IdPessoa == idPessoa)
                    .ToList();

                return pedidosDoUsuario;
            }
            catch (Exception ex)
            {
                // Recomendação: trate ou logue o erro adequadamente, evite throw ex para preservar stack trace
                throw;
            }
        }
    }
}
