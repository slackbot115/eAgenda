using eAgenda.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.ModuloTarefa
{
    public class RepositorioTarefa : RepositorioBase<Tarefa>
    {
        public double ObterCompletude(List<Item> tarefas)
        {
            int contadorPendentes = 0;
            double completude = 0;
            double frequenciaAcrescimo = 100 / tarefas.Count;

            foreach (Item item in tarefas)
            {
                if (item.Status == false)
                    contadorPendentes++;
                else
                    completude += frequenciaAcrescimo;
            }

            return completude;
        }
    }
}
