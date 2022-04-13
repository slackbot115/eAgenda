using eAgenda.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.ModuloTarefa
{
    public class TelaCadastroTarefa : TelaBase
    {
        private RepositorioTarefa repositorioTarefa;
        private RepositorioItem repositorioItem;
        private Notificador notificador;

        public TelaCadastroTarefa(RepositorioTarefa repositorioTarefa, RepositorioItem repositorioItem, Notificador notificador)
            : base("Cadastro de Tarefa")
        {
            this.repositorioTarefa = repositorioTarefa;
            this.repositorioItem = repositorioItem;
            this.notificador = notificador;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para Alterar status dos itens de uma tarefa");
            Console.WriteLine("Digite 6 para Visualizar por prioridade");
            Console.WriteLine("Digite 7 para Visualizar finalizadas");
            Console.WriteLine("Digite 8 para Visualizar tarefas agrupadas");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Tarefa");

            Tarefa novaTarefa = ObterTarefa();

            repositorioTarefa.Inserir(novaTarefa);

            notificador.ApresentarMensagem("Tarefa cadastrada com sucesso!", TipoMensagem.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Tarefa");

            bool temTarefasCadastradas = VisualizarRegistros("Pesquisando");

            if (temTarefasCadastradas == false)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa cadastrada para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroTarefa = ObterNumeroRegistro();

            Tarefa tarefaAtualizada = ObterTarefa();

            bool conseguiuEditar = repositorioTarefa.Editar(numeroTarefa, tarefaAtualizada);

            if (!conseguiuEditar)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Tarefa editada com sucesso!", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Tarefa");

            bool temTarefasRegistradas = VisualizarRegistros("Pesquisando");

            if (temTarefasRegistradas == false)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa cadastrada para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroTarefa = ObterNumeroRegistro();

            if (TarefaPodeExcluir(numeroTarefa) == false)
            {
                notificador.ApresentarMensagem("Tarefas pendentes não podem ser excluidas.", TipoMensagem.Atencao);
                return;
            }

            bool conseguiuExcluir = repositorioTarefa.Excluir(numeroTarefa);

            if (!conseguiuExcluir)
                notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Tarefa excluída com sucesso", TipoMensagem.Sucesso);
        }

        public bool TarefaPodeExcluir(int numeroTarefa)
        {
            Tarefa tarefaSelecionada = repositorioTarefa.SelecionarRegistro(x => x.id == numeroTarefa);
            if(tarefaSelecionada.Status == true)
                return true;

            return false;
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo($"Visualização de Tarefas");

            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Tarefa tarefa in tarefas)
                Console.WriteLine(tarefa.ToString());

            Console.ReadLine();

            return true;
        }

        public bool VisualizarRegistrosSimplificados()
        {
            MostrarTitulo("Visualização de Tarefas");

            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Tarefa tarefa in tarefas)
            {
                Console.WriteLine("ID: " + tarefa.id);
                Console.WriteLine("Título: " + tarefa.Titulo);
                Console.WriteLine("Prioridade: " + tarefa.TipoRelevancia);
                Console.WriteLine("Data de Conclusão: " + tarefa.DataConclusao);
                Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
            }

            Console.ReadLine();

            return true;
        }

        public bool VisualizarTarefasAgrupadas()
        {
            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            MostrarTitulo("Tarefas agrupadas");

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            Console.WriteLine("TAREFAS PENDENTES");
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.Status == false)
                {
                    Console.WriteLine("ID: " + tarefa.id);
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Prioridade: " + tarefa.TipoRelevancia);
                    Console.WriteLine("Data de Conclusão: " + tarefa.DataConclusao);
                    Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
                }
            }

            Console.WriteLine("\nTAREFAS FINALIZADAS");
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.Status == true)
                {
                    Console.WriteLine("ID: " + tarefa.id);
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Prioridade: " + tarefa.TipoRelevancia);
                }
            }

            Console.ReadLine();

            return true;
        }

        public bool VisualizarTarefasFinalizadas()
        {
            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            MostrarTitulo("Tarefas finalizadas");

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.Status == true)
                {
                    Console.WriteLine("ID: " + tarefa.id);
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Prioridade: " + tarefa.TipoRelevancia);
                    Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
                }
            }

            Console.ReadLine();

            return true;
        }

        public bool VisualizarTarefasPorPrioridade()
        {
            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            MostrarTitulo("Tarefas por prioridade (somente pendentes)");

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            Console.WriteLine("PRIORIDADE ALTA");
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.TipoRelevancia == TipoRelevancia.Alta && tarefa.Status == false)
                {
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Data de Conclusão: " + tarefa.DataConclusao);
                    Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
                }
            }
            Console.WriteLine("PRIORIDADE MÉDIA");
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.TipoRelevancia == TipoRelevancia.Media && tarefa.Status == false)
                {
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Data de Conclusão: " + tarefa.DataConclusao);
                    Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
                }
            }
            Console.WriteLine("PRIORIDADE BAIXA");
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.TipoRelevancia == TipoRelevancia.Baixa && tarefa.Status == false)
                {
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Data de Conclusão: " + tarefa.DataConclusao);
                    Console.WriteLine("Ítens: \n" + tarefa.ListarItensTarefa());
                }
            }

            Console.ReadLine();

            return true;
        }

        public void AlterarStatusItens()
        {
            bool temTarefasDisponiveis = VisualizarRegistrosSimplificados();

            if (!temTarefasDisponiveis)
            {
                notificador.ApresentarMensagem("Você precisa cadastrar uma tarefa antes de editar um item!", TipoMensagem.Atencao);
                return;
            }

            Console.Write("Digite o número da tarefa: ");
            int numTarefaSelecionada = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Tarefa tarefaSelecionada = repositorioTarefa.SelecionarRegistro(x => x.id == numTarefaSelecionada);

            List<Item> items = tarefaSelecionada.Itens;

            foreach (Item item in items)
            {
                Console.WriteLine("Deseja alterar o status deste item?");
                Console.WriteLine(item.ToString()); 
                Console.WriteLine("1 - Sim\n0 - Não");
                Console.Write("Digite: ");
                int op = int.Parse(Console.ReadLine());
                if (op == 1)
                    item.AlterarStatus();
            }

            Console.ReadKey();
        }

        private Tarefa ObterTarefa()
        {
            Console.Write("Digite o título da tarefa: ");
            string titulo = Console.ReadLine();

            DateTime dataCriacao = DateTime.Now;
            Console.Write("Data de criação: " + dataCriacao + "\n");

            Console.Write("Digite a data de conclusão: ");
            DateTime dataConclusao = DateTime.Parse(Console.ReadLine());

            Console.Write("Escolha a relevância\n");
            TipoRelevancia tipoRelevancia = ObterTipoRelevancia();

            Console.WriteLine("Cadastrando Items: ");
            List<Item> items = ObterItens();

            Tarefa novaTarefa = new Tarefa(titulo, 
                dataCriacao, 
                dataConclusao,
                tipoRelevancia,
                items
                );

            return novaTarefa;
        }

        private TipoRelevancia ObterTipoRelevancia()
        {
            var tiposRelevanciaListados = Enum.GetValues(typeof(TipoRelevancia)).Cast<TipoRelevancia>().ToArray();
            for (int i = 0; i < tiposRelevanciaListados.Length; i++)
            {
                Console.WriteLine($"{i} - {tiposRelevanciaListados[i]}");
            }
            int relevancia;
            while (true)
            {
                Console.Write("Digite a opção: ");
                relevancia = int.Parse(Console.ReadLine());
                if (relevancia != 0 || relevancia != 1 || relevancia != 2)
                    break;
                else
                    continue;
            }
            TipoRelevancia tipoRelevancia = (TipoRelevancia)relevancia;

            return tipoRelevancia;
        }

        private List<Item> ObterItens()
        {
            RepositorioItem repositorioItem = new RepositorioItem();

            while (true)
            {
                Console.WriteLine("Digite a descrição: ");
                string descricao = Console.ReadLine();

                Item novoItem = new Item(descricao);
                repositorioItem.Inserir(novoItem);

                Console.WriteLine("Item cadastrado com sucesso, deseja criar mais itens?");
                Console.WriteLine("1 - Sim\n0 - Não");
                int op = int.Parse(Console.ReadLine());
                if (op == 1)
                    continue;
                else
                    break;
            }

            return repositorioItem.SelecionarTodos();
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                Console.Write($"Digite o ID da tarefa que deseja editar: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = repositorioTarefa.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    notificador.ApresentarMensagem($"ID da tarefa não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }

    }
}
