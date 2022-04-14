using eAgenda.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.ModuloTarefa
{
    public class Tarefa : EntidadeBase
    {
        private string titulo;
        private DateTime dataCriacao;
        private DateTime dataConclusao;
        private TipoRelevancia tipoRelevancia;
        private double percentualConcluido;
        private bool status = false;
        private List<Item> itens;

        public Tarefa(string titulo,
            DateTime dataConclusao, 
            TipoRelevancia tipoRelevancia, 
            List<Item> itens)
        {
            this.titulo = titulo;
            this.dataCriacao = PassarHora();
            this.dataConclusao = dataConclusao;
            this.tipoRelevancia = tipoRelevancia;
            this.itens = itens;
        }

        private DateTime PassarHora()
        {
            if (dataCriacao == DateTime.MinValue)
            {
                return DateTime.Now;
            }

            return dataCriacao;
        }

        public string Titulo { get => titulo; }
        public DateTime DataCriacao { get => dataCriacao; set => dataCriacao = value; }
        public DateTime DataConclusao { get => dataConclusao; }
        public TipoRelevancia TipoRelevancia { get => tipoRelevancia; }
        public double PercentualConcluido { get => percentualConcluido; }
        public bool Status { get => status; }
        public List<Item> Itens { get => itens; }

        public override string ToString()
        {
            string status = Status ? "Finalizado!" : "Em progresso...";

            return "ID: " + id + Environment.NewLine +
                "Título: " + Titulo + Environment.NewLine +
                "Prioridade: " + TipoRelevancia + Environment.NewLine +
                "Data de Criação: " + DataCriacao + Environment.NewLine +
                "Data de Conclusão: " + DataConclusao + Environment.NewLine +
                "\nItens: \n" + ListarItensTarefa() +
                $"Percentual concluído: {ObterCompletude(Itens)}%" + Environment.NewLine +
                "Status: " + status + Environment.NewLine;
        }

        private double ObterCompletude(List<Item> itensTarefa)
        {
            int contadorPendentes = 0;
            double completude = 0;
            double frequenciaAcrescimo = 100 / itensTarefa.Count;

            foreach (Item item in itensTarefa)
            {
                if (item.Status == false)
                    contadorPendentes++;
                else
                {
                    if(contadorPendentes <= 0)
                        completude += frequenciaAcrescimo;
                    else
                        contadorPendentes--;
                }
            }

            percentualConcluido = completude;
            if (contadorPendentes == 0 || completude == 100)
            {
                status = true;
                percentualConcluido = 100;
            }

            return percentualConcluido;
        }

        public string ListarItensTarefa()
        {
            string itensString = "";

            foreach (Item item in Itens)
            {
                itensString += item.ToString() + "\n";
            }

            return itensString;
        }

    }
}
