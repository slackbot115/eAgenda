using eAgenda.ModuloCompromisso;
using eAgenda.ModuloContato;
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
        private readonly RepositorioItem repositorioItem;
        private ModuloTarefa.TelaCadastroTarefa telaCadastroTarefa;

        private RepositorioContato repositorioContato;
        private TelaCadastroContato telaCadastroContato;

        private RepositorioCompromisso repositorioCompromisso;
        private ModuloCompromisso.TelaCadastroCompromisso telaCadastroCompromisso;

        public TelaMenuPrincipal(Notificador notificador)
        {
            repositorioTarefa = new RepositorioTarefa();
            telaCadastroTarefa = new ModuloTarefa.TelaCadastroTarefa(repositorioTarefa, repositorioItem, notificador);

            repositorioContato = new RepositorioContato();
            telaCadastroContato = new TelaCadastroContato(repositorioContato, notificador);

            repositorioCompromisso = new RepositorioCompromisso();
            telaCadastroCompromisso = new ModuloCompromisso.TelaCadastroCompromisso(repositorioCompromisso, repositorioContato, telaCadastroContato, notificador);
        }

        public string MostrarOpcoes()
        {
            Console.Clear();

            Console.WriteLine("eAgenda");

            Console.WriteLine();

            Console.WriteLine("Digite 1 para Gerenciar Tarefas");
            Console.WriteLine("Digite 2 para Gerenciar Contatos");
            Console.WriteLine("Digite 3 para Gerenciar Compromissos");

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
            else if (opcao == "2")
                tela = telaCadastroContato;
            else if (opcao == "3")
                tela = telaCadastroCompromisso;

            return tela;
        }
    }

}
