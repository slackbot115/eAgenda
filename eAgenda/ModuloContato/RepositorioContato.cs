using eAgenda.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.ModuloContato
{
    public class RepositorioContato : RepositorioBase<Contato>
    {
        public IEnumerable<IGrouping<string, Contato>> SelecionarContatosPorCargo()
        {
            return SelecionarTodos().GroupBy(x => x.Cargo);
        }
    }
}
