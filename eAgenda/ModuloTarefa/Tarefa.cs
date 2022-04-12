﻿using eAgenda.Compartilhado;
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
        private int percentualConcluido;
        private bool status = false;
        private List<Item> itens;

        public Tarefa(string titulo, 
            DateTime dataCriacao, 
            DateTime dataConclusao, 
            TipoRelevancia tipoRelevancia, 
            List<Item> itens)
        {
            this.titulo = titulo;
            this.dataCriacao = dataCriacao;
            this.dataConclusao = dataConclusao;
            this.tipoRelevancia = tipoRelevancia;
            this.itens = itens;
        }

        public string Titulo { get => titulo; }
        public DateTime DataCriacao { get => dataCriacao; }
        public DateTime DataConclusao { get => dataConclusao; }
        public TipoRelevancia TipoRelevancia { get => tipoRelevancia; }
        public int PercentualConcluido { get => percentualConcluido; }
        public bool Status { get => status; }
        public List<Item> Itens { get => itens; }

        public override string ToString()
        {
            return "ID: " + id + Environment.NewLine +
                "Título: " + Titulo + Environment.NewLine +
                "Prioridade: " + TipoRelevancia + Environment.NewLine +
                "Data de Criação: " + DataCriacao + Environment.NewLine +
                "Data de Conclusão: " + DataConclusao + Environment.NewLine +
                "\nItens: \n" + ListarItensTarefa() +
                $"Percentual concluído: {PercentualConcluido}%" + Environment.NewLine +
                "Finalizado?: " + Status + Environment.NewLine;
        }

        private string ListarItensTarefa()
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