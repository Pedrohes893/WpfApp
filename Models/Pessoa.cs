using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Validacoes;

namespace WpfApp.Models
{
    public class Pessoa
    {
        public int IdPessoa { get; set; }
        public string NomePessoa { get; set; }
        public string NumCpf { get; set; }
        public string Endereco { get; set; }

    }

}
