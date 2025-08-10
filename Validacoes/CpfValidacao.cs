using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WpfApp.Validacoes
{
    public class CpfAttribute : ValidationAttribute
    {
        public bool IsValid(object value)
        {
            if (value == null) return true; // Permite null, pois o [Required] trata obrigatoriedade

            var cpf = value.ToString().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11 || !Regex.IsMatch(cpf, @"^\d{11}$"))
                return false;

            // Verifica CPFs inválidos conhecidos
            if (new string(cpf[0], cpf.Length) == cpf)
                return false;

            // Validação dos dígitos verificadores
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
