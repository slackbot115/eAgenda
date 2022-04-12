using eAgenda.Compartilhado;
using System;
using System.Collections.Generic;

namespace eAgenda
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Notificador notificador = new Notificador();
            TelaMenuPrincipal menuPrincipal = new TelaMenuPrincipal(notificador);

            while (true)
            {
                TelaBase telaSelecionada = menuPrincipal.ObterTela();

                if (telaSelecionada is null)
                    return;

                string opcaoSelecionada = telaSelecionada.MostrarOpcoes();

                if (telaSelecionada is ITelaCadastravel)
                {
                    ITelaCadastravel telaCadastravel = (ITelaCadastravel)telaSelecionada;

                    if (opcaoSelecionada == "1")
                        telaCadastravel.Inserir();

                    else if (opcaoSelecionada == "2")
                        telaCadastravel.Editar();

                    else if (opcaoSelecionada == "3")
                        telaCadastravel.Excluir();

                    else if (opcaoSelecionada == "4")
                        telaCadastravel.VisualizarRegistros("Tela");
                }
            }
        }
    }
}
