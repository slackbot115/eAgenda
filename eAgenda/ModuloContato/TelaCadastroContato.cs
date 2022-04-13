using eAgenda.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.ModuloContato
{
    public class TelaCadastroContato : TelaBase, ITelaCadastravel
    {
        private readonly RepositorioContato repositorioContato;
        private readonly Notificador notificador;

        public TelaCadastroContato(RepositorioContato repositorioContato, Notificador notificador)
            : base("Cadastro de Contatos")
        {
            this.repositorioContato = repositorioContato;
            this.notificador = notificador;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Contato");

            Contato novoContato = ObterContato();

            repositorioContato.Inserir(novoContato);

            notificador.ApresentarMensagem("Contato cadastrado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Contato");

            bool temContatosCadastrados = VisualizarRegistros("Pesquisando");

            if (temContatosCadastrados == false)
            {
                notificador.ApresentarMensagem("Nenhum contato cadastrado para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroContato = ObterNumeroRegistro();

            Contato contatoAtualizado = ObterContato();

            bool conseguiuEditar = repositorioContato.Editar(numeroContato, contatoAtualizado);

            if (!conseguiuEditar)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Contato editado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Contato");

            bool temContatosRegistrados = VisualizarRegistros("Pesquisando");

            if (temContatosRegistrados == false)
            {
                notificador.ApresentarMensagem("Nenhum contato cadastrado para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroContato = ObterNumeroRegistro();

            bool conseguiuExcluir = repositorioContato.Excluir(numeroContato);

            if (!conseguiuExcluir)
                notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Contato excluído com sucesso!", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Fornecedores");

            List<Contato> contatos = repositorioContato.SelecionarTodos();

            if (contatos.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhum contato disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Contato contato in contatos)
                Console.WriteLine(contato.ToString());

            Console.ReadLine();

            return true;
        }

        private Contato ObterContato()
        {
            Console.Write("Digite o nome do contato: ");
            string nome = Console.ReadLine();
            
            Console.Write("Digite o email do contato: ");
            string email = Console.ReadLine();

            Console.Write("Digite o telefone do contato: ");
            string telefone = Console.ReadLine();

            Console.Write("Digite a empresa do contato: ");
            string empresa = Console.ReadLine();

            Console.Write("Digite o cargo do contato: ");
            string cargo = Console.ReadLine();

            return new Contato(nome, telefone, email, empresa, cargo);
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                Console.Write("Digite o ID do contato que deseja editar: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = repositorioContato.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    notificador.ApresentarMensagem("ID do contato não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }
    }
}
