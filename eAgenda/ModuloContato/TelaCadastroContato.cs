using eAgenda.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.ModuloContato
{
    public class TelaCadastroContato : TelaBase
    {
        private readonly RepositorioContato repositorioContato;
        private readonly Notificador notificador;

        public TelaCadastroContato(RepositorioContato repositorioContato, Notificador notificador)
            : base("Cadastro de Contatos")
        {
            this.repositorioContato = repositorioContato;
            this.notificador = notificador;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para Visualizar contatos por cargo");

            Console.WriteLine("Digite s para sair");

            string opcao = MetodosAuxiliares.ValidarInputString("Opção: ");

            return opcao;
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

        public void VisualizarContatosPorCargo()
        {
            List<Contato> contatos = repositorioContato.SelecionarTodos();

            IEnumerable<IGrouping<string, Contato>> contatosPorCargo =
                repositorioContato.SelecionarContatosPorCargo();

            foreach (var cargos in contatosPorCargo)
            {
                Console.WriteLine("\nCargo: " + cargos.Key);

                foreach (var contato in contatos)
                {
                    if (contato.Cargo == cargos.Key)
                    {
                        Console.WriteLine("\tID: " + contato.id);
                        Console.WriteLine("\tNome: " + contato.Nome);
                        Console.WriteLine("\tEmail: " + contato.Email);
                        Console.WriteLine("\tTelefone: " + contato.Telefone);
                        Console.WriteLine("\tEmpresa: " + contato.Empresa);
                        Console.WriteLine();
                    }
                }
            }

            Console.ReadKey();
        }

        private Contato ObterContato()
        {
            string nome = MetodosAuxiliares.ValidarInputString("Digite o nome do contato: ");

            string email = MetodosAuxiliares.ValidarEmail();

            string telefone = MetodosAuxiliares.ValidarNumeroTelefone();

            string empresa = MetodosAuxiliares.ValidarInputString("Digite a empresa do contato: ");

            string cargo = MetodosAuxiliares.ValidarInputString("Digite o cargo do contato: ");

            return new Contato(nome, email, telefone, empresa, cargo);
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                numeroRegistro = MetodosAuxiliares.ValidarInputInt("Digite o ID do contato que deseja editar: ");

                numeroRegistroEncontrado = repositorioContato.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    notificador.ApresentarMensagem("ID do contato não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }
    }
}
