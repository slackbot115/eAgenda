using eAgenda.Compartilhado;
using eAgenda.ModuloContato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.ModuloCompromisso
{
    public class TelaCadastroCompromisso : TelaBase
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

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para Alterar status de um compromisso");
            Console.WriteLine("Digite 6 para Visualizar compromissos agrupados");
            Console.WriteLine("Digite 7 para Visualizar por período");

            Console.WriteLine("Digite s para sair");

            string opcao = MetodosAuxiliares.ValidarInputString("Opção: ");

            return opcao;
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

        public bool VisualizarCompromissosPorPeriodo()
        {
            List<Compromisso> compromissos = repositorioCompromisso.SelecionarTodos();

            if (compromissos.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhum compromisso disponível.", TipoMensagem.Atencao);
                return false;
            }

            Console.WriteLine("Como deseja visualiar os compromissos futuros?");
            Console.WriteLine("1 - No dia atual");
            Console.WriteLine("2 - Na semana atual");
            Console.WriteLine("3 - No mês atual");
            Console.WriteLine("4 - No ano atual");
            Console.WriteLine("5 - Entre certo periodo");
            string op = MetodosAuxiliares.ValidarInputString("Digite a opção desejada: ");
            switch (op)
            {
                case "1":
                    VisualizarCompromissosDiarios();
                    break;
                case "2":
                    VisualizarCompromissosSemanais();
                    break;
                case "3":
                    VisualizarCompromissosMensais();
                    break;
                case "4":
                    VisualizarCompromissosAnuais();
                    break;
                case "5":
                    VisualizarCompromissoEmPeriodoEspecifico();
                    break;
            }

            Console.ReadKey();
            return true;
        }

        private void VisualizarCompromissoEmPeriodoEspecifico()
        {
            List<Compromisso> compromissos = repositorioCompromisso.SelecionarTodos();

            Console.WriteLine("Digite a data inicial do periodo: ");
            DateTime dataInicial = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Digite a data final do periodo: ");
            DateTime dataFinal = DateTime.Parse(Console.ReadLine());

            Console.WriteLine($"Compromissos entre {dataInicial} e {dataFinal}: \n");
            foreach (Compromisso compromisso in compromissos)
            {
                if (compromisso.DataCompromisso > dataInicial && compromisso.DataCompromisso < dataFinal)
                    Console.WriteLine(compromisso.ToString());
            }
        }

        private void VisualizarCompromissosAnuais()
        {
            List<Compromisso> compromissos = repositorioCompromisso.SelecionarTodos();

            Console.WriteLine("Compromissos do ano: \n");
            foreach (Compromisso compromisso in compromissos)
            {
                if (compromisso.DataCompromisso.Year == DateTime.Now.Year)
                    Console.WriteLine(compromisso.ToString());
            }
        }

        private void VisualizarCompromissosMensais()
        {
            List<Compromisso> compromissos = repositorioCompromisso.SelecionarTodos();

            Console.WriteLine("Compromissos do mês: \n");
            foreach (Compromisso compromisso in compromissos)
            {
                if (compromisso.DataCompromisso.Month == DateTime.Now.Month)
                    Console.WriteLine(compromisso.ToString());
            }
        }

        private void VisualizarCompromissosSemanais()
        {
            List<Compromisso> compromissos = repositorioCompromisso.SelecionarTodos();

            Console.WriteLine("Compromissos da semana: \n");
            foreach (Compromisso compromisso in compromissos)
            {
                if (compromisso.DataCompromisso.AddDays((double)(7 - compromisso.DataCompromisso.DayOfWeek)).Date.Equals(DateTime.Now.AddDays((double)(7 - DateTime.Now.DayOfWeek)).Date))
                    Console.WriteLine(compromisso.ToString());
            }
        }

        private void VisualizarCompromissosDiarios()
        {
            List<Compromisso> compromissos = repositorioCompromisso.SelecionarTodos();

            Console.WriteLine("Compromissos do dia: \n");
            foreach (Compromisso compromisso in compromissos)
            {
                if (compromisso.DataCompromisso.Day == DateTime.Now.Day)
                    Console.WriteLine(compromisso.ToString());
            }
        }

        public bool VisualizarCompromissosAgrupados()
        {
            List<Compromisso> compromissos = repositorioCompromisso.SelecionarTodos();

            MostrarTitulo("Compromissos agrupados");

            if (compromissos.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhum compromisso disponível.", TipoMensagem.Atencao);
                return false;
            }

            Console.WriteLine("COMPROMISSOS PENDENTES");
            foreach (Compromisso compromisso in compromissos)
            {
                if (compromisso.Status == false)
                {
                    Console.WriteLine("ID: " + compromisso.id);
                    Console.WriteLine("Assunto: " + compromisso.Assunto);
                    Console.WriteLine("Local: " + compromisso.Local);
                    Console.WriteLine("Contato: " + compromisso.Contato.Nome);
                    Console.WriteLine("Data do Compromisso: " + compromisso.DataCompromisso);
                    Console.WriteLine($"Hora de Início: {compromisso.HoraInicio}\t Hora de Término {compromisso.HoraTermino}");
                }
            }

            Console.WriteLine("\nCOMPROMISSOS FINALIZADOS");
            foreach (Compromisso compromisso in compromissos)
            {
                if (compromisso.Status == true)
                {
                    Console.WriteLine("ID: " + compromisso.id);
                    Console.WriteLine("Assunto: " + compromisso.Assunto);
                    Console.WriteLine("Contato: " + compromisso.Contato.Nome);
                    Console.WriteLine("Data do Compromisso: " + compromisso.DataCompromisso);
                }
            }

            Console.ReadLine();

            return true;
        }

        public void AlterarStatusCompromisso()
        {
            bool temCompromissosDisponiveis = telaCadastroContato.VisualizarRegistros("");

            if (!temCompromissosDisponiveis)
            {
                notificador.ApresentarMensagem("Você precisa cadastrar um compromisso antes de alterar seu status!", TipoMensagem.Atencao);
                return;
            }

            int numCompromissoSelecionado = MetodosAuxiliares.ValidarInputInt("Digite o ID do compromisso: ");

            Console.WriteLine();

            Compromisso compromissoSelecionado = repositorioCompromisso.SelecionarRegistro(x => x.id == numCompromissoSelecionado);

            compromissoSelecionado.AlterarStatusCompromisso();

            notificador.ApresentarMensagem("Status de compromisso alterado com sucesso!", TipoMensagem.Sucesso);
        }

        private Compromisso ObterCompromisso(Contato contato)
        {
            string assunto = MetodosAuxiliares.ValidarInputString("Digite o assunto do compromisso: ");

            string local = MetodosAuxiliares.ValidarInputString("Digite o local do compromisso: ");

            DateTime dataCompromisso = MetodosAuxiliares.ValidarInputDate("Digite a data do compromisso: ");

            string horaInicio = MetodosAuxiliares.ValidarInputString("Digite a hora de inicio do compromisso: ");

            string horaTermino = MetodosAuxiliares.ValidarInputString("Digite a hora de termino do compromisso: ");

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

            int numContatoSelecionado = MetodosAuxiliares.ValidarInputInt("Digite o ID do contato: ");

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
                numeroRegistro = MetodosAuxiliares.ValidarInputInt("Digite o ID do compromisso que deseja editar: ");

                numeroRegistroEncontrado = repositorioCompromisso.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    notificador.ApresentarMensagem("ID do compromisso não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }
    }
}
