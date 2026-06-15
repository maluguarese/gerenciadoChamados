using System;
using System.Collections.Generic;
using System.Globalization;
using gerenciadorChamados.Dominio;

namespace gerenciadorChamados.UI
{
    internal class MenuConsole
    {
        private readonly GerenciadorChamados _gerenciador;

        public MenuConsole(GerenciadorChamados gerenciador)
        {
            _gerenciador = gerenciador;
        }

        public void Executar()
        {
            bool sair = false;

            while (!sair)
            {
                ExibirMenu();

                Console.Write("Escolha uma opção: ");
                string? opc = Console.ReadLine();

                switch (opc)
                {
                    case "1":
                        ExibirNovoChamado();
                        break;
                    case "2":
                        ExibirLista();
                        break;
                    case "3":
                        ExibirDetalhe();
                        break;
                    case "0":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        AguardarTecla();
                        break;
                }
            }
        }

        private void ExibirMenu()
        {
            Console.Clear();
            Console.WriteLine("=== SISTEMA DE GERENCIAMENTO DE CHAMADOS ===");
            Console.WriteLine("1 - Abrir novo chamado");
            Console.WriteLine("2 - Listar chamados registrados");
            Console.WriteLine("3 - Exibir detalhes do chamado");
            Console.WriteLine("0 - Sair");
            Console.WriteLine();
        }

        private void ExibirNovoChamado()
        {
            Console.Clear();
            Console.WriteLine("=== ABERTURA DE NOVO CHAMADO ===");
            Console.Write("Descrição do chamado: ");
            string? descricao = Console.ReadLine();

            try
            {
                var chamado = _gerenciador.AbrirChamado(descricao ?? string.Empty);
                Console.WriteLine($"Chamado criado com o ID {chamado.Id}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            AguardarTecla();
        }

        private void ExibirLista()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE CHAMADOS REGISTRADOS ===");

            var lista = _gerenciador.ListarTodos();

            if (lista.Count == 0)
            {
                Console.WriteLine("Não há chamados registrados.");
            }
            else
            {
                foreach (var c in lista)
                {
                    Console.WriteLine($"ID: {c.Id} | {c.Status} | {c.Descricao} | Abertura: {c.DataAbertura.ToString("g", CultureInfo.CurrentCulture)}");
                }
            }

            AguardarTecla();
        }

        private void ExibirDetalhe()
        {
            Console.Clear();
            Console.WriteLine("=== DETALHES DO CHAMADO ===");
            Console.Write("Informe o ID do chamado: ");
            string? entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out int id))
            {
                Console.WriteLine("ID inválido.");
                AguardarTecla();
                return;
            }

            var chamado = _gerenciador.ObterPorId(id);

            if (chamado == null)
            {
                Console.WriteLine("Chamado não encontrado.");
                AguardarTecla();
                return;
            }

            Console.WriteLine($"ID: {chamado.Id}");
            Console.WriteLine($"Descrição: {chamado.Descricao}");
            Console.WriteLine($"Status: {chamado.Status}");
            Console.WriteLine($"Abertura: {chamado.DataAbertura.ToString("f", CultureInfo.CurrentCulture)}");
            if (chamado.DataConclusao != null)
            {
                Console.WriteLine($"Conclusão: {chamado.DataConclusao.Value.ToString("f", CultureInfo.CurrentCulture)}");
            }

            if (chamado.EstaAberto())
            {
                Console.WriteLine();
                Console.WriteLine("1 - Concluir o chamado");
                Console.WriteLine("Outra tecla - Voltar");
                Console.Write("Opção: ");
                string? op = Console.ReadLine();

                if (op == "1")
                {
                    bool ok = _gerenciador.ConcluirChamado(chamado.Id);
                    Console.WriteLine(ok ? "Chamado concluído com sucesso." : "Não foi possível concluir o chamado.");
                }
            }

            AguardarTecla();
        }

        private void AguardarTecla()
        {
            Console.WriteLine();
            Console.WriteLine("Pressione Enter para continuar...");
            Console.ReadLine();
        }
    }
}
