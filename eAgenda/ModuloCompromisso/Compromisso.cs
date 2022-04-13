using eAgenda.Compartilhado;
using eAgenda.ModuloContato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.ModuloCompromisso
{
    public class Compromisso : EntidadeBase
    {
        private string assunto;
        private string local;
        private DateTime dataCompromisso;
        private string horaInicio;
        private string horaTermino;
        private Contato contato;

        public Compromisso(string assunto, string local, DateTime dataCompromisso, string horaInicio, string horaTermino, Contato contato)
        {
            this.assunto = assunto;
            this.local = local;
            this.dataCompromisso = dataCompromisso;
            this.horaInicio = horaInicio;
            this.horaTermino = horaTermino;
            this.contato = contato;
        }

        public string Assunto { get => assunto; set => assunto = value; }
        public string Local { get => local; set => local = value; }
        public DateTime DataCompromisso { get => dataCompromisso; set => dataCompromisso = value; }
        public string HoraInicio { get => horaInicio; set => horaInicio = value; }
        public string HoraTermino { get => horaTermino; set => horaTermino = value; }
        public Contato Contato { get => contato; set => contato = value; }

        public override string ToString()
        {
            return "ID: " + id + Environment.NewLine +
                "Assunto: " + Assunto + Environment.NewLine +
                "Local: " + Local + Environment.NewLine +
                "Contato: " + Contato.Nome + Environment.NewLine +
                "Data do Compromisso: " + DataCompromisso + Environment.NewLine +
                "Hora de Inicio: " + HoraInicio + Environment.NewLine +
                "Hora de Termino: " + HoraTermino + Environment.NewLine;
        }
    }
}
