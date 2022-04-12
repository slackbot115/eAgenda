using eAgenda.ModuloTarefa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Compartilhado
{
    public class TelaMenuPrincipal
    {
        private RepositorioTarefa repositorioTarefa;
        private RepositorioItem repositorioItem;
        private TelaCadastroTarefa telaCadastroTarefa;

        public TelaMenuPrincipal(Notificador notificador)
        {
            repositorioTarefa = new RepositorioTarefa();
            telaCadastroTarefa = new TelaCadastroTarefa(repositorioTarefa, repositorioItem, notificador);
        }

        public string MostrarOpcoes()
        {
            Console.Clear();

            Console.WriteLine("eAgenda");

            Console.WriteLine();

            Console.WriteLine("Digite 1 para Gerenciar Tarefas");

            Console.WriteLine("Digite s para sair");

            string opcaoSelecionada = Console.ReadLine();

            return opcaoSelecionada;
        }

        public TelaBase ObterTela()
        {
            string opcao = MostrarOpcoes();

            TelaBase tela = null;

            if (opcao == "1")
                tela = telaCadastroTarefa;

            return tela;
        }
    }

}
