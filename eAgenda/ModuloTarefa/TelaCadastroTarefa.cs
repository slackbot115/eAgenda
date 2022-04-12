using eAgenda.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.ModuloTarefa
{
    public class TelaCadastroTarefa : TelaBase, ITelaCadastravel
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

            bool conseguiuExcluir = repositorioTarefa.Excluir(numeroTarefa);

            if (!conseguiuExcluir)
                notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Tarefa excluída com sucesso", TipoMensagem.Sucesso);
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

        private Tarefa ObterTarefa()
        {
            Console.Write("Digite o título da tarefa: ");
            string titulo = Console.ReadLine();

            DateTime dataCriacao = DateTime.Now;
            Console.Write("Data de criação: " + dataCriacao + "\n");

            Console.Write("Digite a data de conclusão: ");
            DateTime dataConclusao = DateTime.Parse(Console.ReadLine());

            Console.Write("Escolha a relevância\n");
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

        private void ExibirTiposRelevancia()
        {
            var valoresEnum = Enum.GetValues(typeof(TipoRelevancia));


        }

        public List<Item> ObterItens()
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
