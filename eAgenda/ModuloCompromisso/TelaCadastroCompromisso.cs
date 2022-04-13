using eAgenda.Compartilhado;
using eAgenda.ModuloContato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.ModuloCompromisso
{
    public class TelaCadastroCompromisso : TelaBase, ITelaCadastravel
    {
        private readonly RepositorioCompromisso repositorioCompromisso;
        private readonly Notificador notificador;

        private RepositorioContato repositorioContato;
        private TelaCadastroContato telaCadastroContato;

        public TelaCadastroCompromisso(RepositorioCompromisso repositorioCompromisso, RepositorioContato repositorioContato, TelaCadastroContato telaCadastroContato, Notificador notificador)
            : base("Cadastro de Compromissos")
        {
            this.repositorioCompromisso = repositorioCompromisso;
            this.repositorioContato = repositorioContato;
            this.telaCadastroContato = telaCadastroContato;
            this.notificador = notificador;
        }

        public void Inserir()
        {
            MostrarTitulo("Inserindo Compromisso");

            Contato contatoSelecionado = ObterContato();

            if (contatoSelecionado == null)
            {
                notificador
                    .ApresentarMensagem("Cadastre um contato antes de cadastrar compromissos!", TipoMensagem.Atencao);
                return;
            }

            Compromisso novoCompromisso = ObterCompromisso(contatoSelecionado);

            string statusValidacao = repositorioCompromisso.Inserir(novoCompromisso);

            if (statusValidacao == "REGISTRO_VALIDO")
                notificador.ApresentarMensagem("Compromisso inserido com sucesso", TipoMensagem.Sucesso);
            else
                notificador.ApresentarMensagem(statusValidacao, TipoMensagem.Erro);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Compromisso");

            bool temCompromissosCadastrados = VisualizarRegistros("Pesquisando");

            if (temCompromissosCadastrados == false)
            {
                notificador.ApresentarMensagem("Nenhum compromisso cadastrado para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroCompromisso = ObterNumeroRegistro();

            Console.WriteLine();

            Contato contatoSelecionado = ObterContato();

            Compromisso compromissoAtualizado = ObterCompromisso(contatoSelecionado);

            bool conseguiuEditar = repositorioCompromisso.Editar(numeroCompromisso, compromissoAtualizado);

            if (!conseguiuEditar)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Compromisso editado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Compromisso");

            bool temCompromissosRegistrados = VisualizarRegistros("Pesquisando");

            if (temCompromissosRegistrados == false)
            {
                notificador.ApresentarMensagem("Nenhum compromisso cadastrado para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroCompromisso = ObterNumeroRegistro();

            bool conseguiuExcluir = repositorioCompromisso.Excluir(numeroCompromisso);

            if (!conseguiuExcluir)
                notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Compromisso excluído com sucesso!", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Compromissos");

            List<Compromisso> compromissos = repositorioCompromisso.SelecionarTodos();

            if (compromissos.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhum compromisso disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Compromisso compromisso in compromissos)
                Console.WriteLine(compromisso.ToString());

            Console.ReadLine();

            return true;
        }

        private Compromisso ObterCompromisso(Contato contato)
        {
            Console.Write("Digite o assunto do compromisso: ");
            string assunto = Console.ReadLine();

            Console.Write("Digite o local do compromisso: ");
            string local = Console.ReadLine();

            Console.Write("Digite a data do compromisso: ");
            DateTime dataCompromisso = DateTime.Parse(Console.ReadLine());

            Console.Write("Digite a hora de inicio do compromisso: ");
            string horaInicio = Console.ReadLine();

            Console.Write("Digite a hora de termino do compromisso: ");
            string horaTermino = Console.ReadLine();

            return new Compromisso(assunto, local, dataCompromisso, horaInicio, horaTermino, contato);
        }

        public Contato ObterContato()
        {
            bool temContatosDisponiveis = telaCadastroContato.VisualizarRegistros("");

            if (!temContatosDisponiveis)
            {
                notificador.ApresentarMensagem("Você precisa cadastrar um contato antes de um compromisso!", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o ID do contato: ");
            int numContatoSelecionado = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Contato contatoSelecionado = repositorioContato.SelecionarRegistro(x => x.id == numContatoSelecionado);

            return contatoSelecionado;
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                Console.Write("Digite o ID do compromisso que deseja editar: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = repositorioCompromisso.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    notificador.ApresentarMensagem("ID do compromisso não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }
    }
}
