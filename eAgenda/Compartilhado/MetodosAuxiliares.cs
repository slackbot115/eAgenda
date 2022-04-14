using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eAgenda.Compartilhado
{
    public class MetodosAuxiliares
    {
        public static string ValidarHorario(string mensagem)
        {
            string horas;
            while (true)
            {
                Console.Write(mensagem);
                horas = Console.ReadLine();
                try
                {
                    if (FormatarHoras(horas) != null)
                    {
                        return FormatarHoras(horas);
                    }
                    else
                    {
                        Console.WriteLine("Horario inválido, digite novamente...");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Input inválido, tente novamente...");
                }
            }
        }

        private static string FormatarHoras(string horas)
        {
            var verificarRegex = Regex.Match(horas, @"^([0-1][0-9]|2[0-4])\:[0-5][0-9]$");
            if (verificarRegex.Success)
            {
                return Regex.Replace(horas, @"^(\d{2})[ -]?(\d{5})[ -]?(\d{4})", @"$1:$2");
            }
            else
            {
                return null;
            }
        }

        public static string ValidarEmail()
        {
            string email;
            while (true)
            {
                Console.Write("Digite o email (emails validos somente com dominio): ");
                email = Console.ReadLine();
                try
                {
                    if (FormatarEmail(email) != null)
                    {
                        return FormatarEmail(email);
                    }
                    else
                    {
                        Console.WriteLine("Email inválido, digite novamente...");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Input inválido, tente novamente...");
                }
            }
        }

        private static string FormatarEmail(string email)
        {
            var verificarRegex = Regex.Match(email, @"^[a-z0-9.]+@[a-z0-9]+\.[a-z]+(\.[a-z]+)?$");
            if (verificarRegex.Success)
            {
                return Regex.Replace(email, @"^(\d{2})[ -]?(\d{5})[ -]?(\d{4})", @"($1) $2-$3");
            }
            else
            {
                return null;
            }
        }

        public static string ValidarNumeroTelefone()
        {
            string numero;
            while (true)
            {
                Console.Write("Digite o número de telefone (DDD + número, sem espaços e parênteses) : ");
                numero = Console.ReadLine();
                numero = numero.Replace(" ", "");
                try
                {
                    if (numero.Length == 11)
                    {
                        return FormatarNumeroTelefone(numero);
                    }
                    else
                    {
                        Console.WriteLine("Digite novamente...");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Input inválido, tente novamente...");
                }
            }
        }

        private static string FormatarNumeroTelefone(string numero)
        {
            string numeroSemLetras = RemoverLetras(numero);
            var verificarRegex = Regex.Match(numeroSemLetras, @"^\(?[1-9]{2}\)? ?(?:[2-8]|9[1-9])[0-9]{3}\-?[0-9]{4}$");
            if (verificarRegex.Success)
            {
                return Regex.Replace(numeroSemLetras, @"^(\d{2})[ -]?(\d{5})[ -]?(\d{4})", @"($1) $2-$3");
            }
            else
            {
                return numeroSemLetras;
            }
        }

        public static string RemoverLetras(string palavra)
        {
            string[] charsToRemove = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            foreach (var c in charsToRemove)
            {
                palavra = palavra.Replace(c, string.Empty);
            }
            return palavra;
        }

        public static string ValidarInputString(string mensagem)
        {
            string palavra;
            while (true)
            {
                Console.Write(mensagem);
                palavra = Console.ReadLine();
                try
                {
                    if (palavra.Length > 0)
                    {
                        return palavra;
                    }
                    else
                    {
                        Console.WriteLine("Digite novamente...");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Input inválido, tente novamente...");
                }
            }
        }

        public static int ValidarInputInt(string mensagem)
        {
            string numero;
            while (true)
            {
                Console.Write(mensagem);
                numero = Console.ReadLine();
                try
                {
                    if (numero.Length > 0)
                    {
                        return int.Parse(numero);
                    }
                    else
                    {
                        Console.WriteLine("Digite novamente...");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Input inválido, tente novamente...");
                }
            }
        }

        public static DateTime ValidarInputDate(string mensagem)
        {
            DateTime data;
            while (true)
            {
                Console.Write(mensagem);
                try
                {
                    data = DateTime.Parse(Console.ReadLine());
                    if(data.Day > DateTime.Now.Day)
                        return data;
                    else
                    {
                        Console.WriteLine("Data inválida, digite novamente...");
                    }
                }
                catch
                {
                    Console.WriteLine("Input inválido, tente novamente...");
                }
            }
        }

    }
}
