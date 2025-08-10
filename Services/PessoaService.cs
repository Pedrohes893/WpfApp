using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfApp.Models;
using WpfApp.Validacoes;

namespace WpfApp.Services
{

    public class PessoaService
    {
        private readonly string caminhoJson;
        public PessoaService() {
            // Caminho até o diretório do projeto
            string pastaProjeto = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
            caminhoJson = Path.Combine(pastaProjeto, "Data", "pessoas.json");
        }

        public async Task<List<Pessoa>> CarregarPessoasAsync(string nomePessoa)
        {
            try
            {
                if (!File.Exists(caminhoJson))
                    return new List<Pessoa>();

                string json = await File.ReadAllTextAsync(caminhoJson);
                var lista = JsonSerializer.Deserialize<List<Pessoa>>(json);

                if (!string.IsNullOrWhiteSpace(nomePessoa))
                {
                    // Filtra a lista, comparando o nome (ignora maiúsculas/minúsculas)
                    lista = lista
                        .Where(p => p.NomePessoa.ToUpper().Contains(nomePessoa.ToUpper()))
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
        public async Task SalvarPessoasAsync(List<Pessoa> pessoas)
        {

            try
            {

                CpfAttribute validador = new CpfAttribute();

                foreach (var pessoa in pessoas)
                {
                    if (string.IsNullOrEmpty(pessoa.NumCpf))
                    {
                        throw new ArgumentException("O CPF é obrigatório!");
                    }
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(pessoas, options);
                await File.WriteAllTextAsync(caminhoJson, json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
